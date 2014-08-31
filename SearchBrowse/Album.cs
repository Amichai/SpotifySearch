using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SearchBrowse {
    public class Album {
        public Album() {
            this.Duration = TimeSpan.FromSeconds(0);
            this.TrackCount = 0;
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Artist { get; set; }
        public string ID { get; set; }
        public TimeSpan Duration { get; set; }
        public int TrackCount { get; set; }

        static string urlBase = @"https://api.spotify.com";


        internal static Album Parse(JToken i) {
            Album toReturn = new Album();
            toReturn.ID = i["id"].ToString();
            toReturn.ImageUrl = i["images"].First["url"].ToString();
            toReturn.Name = i["name"].ToString();

            toReturn.loadAlbumData();
            return toReturn;
        }

        private void loadAlbumData() {
            string url = string.Format("{0}/v1/albums/{1}", urlBase, this.ID);
            using (WebClient client = new WebClient()) {
                string s = client.DownloadString(url);
                JObject j = JObject.Parse(s);
                this.Artist = j["artists"].First["name"].ToString();

                foreach (var t in j["tracks"]["items"]) {
                    int milli = int.Parse(t["duration_ms"].ToString());
                    this.Duration += TimeSpan.FromMilliseconds(milli);
                    this.TrackCount++;
                }
            }
        }
    }
}
