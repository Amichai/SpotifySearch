using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBrowse {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();

            this.SearchTerm = "Unabridged";
            this.Albums = new ObservableCollection<Album>();
            this.search();
        }


        string urlBase = @"https://api.spotify.com";

        private void search() {
            int limit = 50;
            this.Albums.Clear();
            for (int j = 0; j < 3; j++) {    
                int offset = limit * j;
                string url = string.Format("{0}/v1/search?q={1}&type=album&limit={2}&offset={3}", urlBase, this.SearchTerm, limit, offset);
                List<Album> toAdd = new List<Album>();
                using (WebClient client = new WebClient()) {
                    string s = client.DownloadString(url);
                    //string s = System.IO.File.ReadAllText(@"..\..\sample.txt");
                    var r = JObject.Parse(s);
                    foreach (var i in r["albums"]["items"]) {
                        Album a = Album.Parse(i);
                        toAdd.Add(a);
                    }
                }
                foreach (var a in toAdd.OrderByDescending(i => i.Duration).ToList()) {
                    this.Albums.Add(a);
                }
            }
        }

        private ObservableCollection<Album> _Albums;
        public ObservableCollection<Album> Albums {
            get { return _Albums; }
            set {
                _Albums = value;
                NotifyPropertyChanged();
            }
        }

        private string _SearchTerm;
        public string SearchTerm {
            get { return _SearchTerm; }
            set {
                _SearchTerm = value;
                NotifyPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.search();
        }

        
    }
}
