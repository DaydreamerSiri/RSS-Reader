using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using System.ComponentModel;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GridViewColumnHeader lastHeaderClicked = null;
        private ListSortDirection lastDirection = ListSortDirection.Ascending;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenBrowser((sender as Button).Tag as string);
        }

        private void GoToUpdate_DoubleClick(object sender, RoutedEventArgs e)
        {
            this.OpenBrowser((sender as TextBlock).Tag as string);
        }

        private void OpenBrowser(string Path)
        {
            try
            {
                Process.Start(Path);
            }
            catch
            {
                // hack wegen dieses Fehlers: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Path = Path.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {Path}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", Path);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", Path);
                }
                else
                {
                    MessageBox.Show("Bitte nutzen sie einen unterstütztes System (Win/Lin/Mac)", "Falsches Betriebssystem", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        private void FilterBox_TextChanged(object sender, RoutedEventArgs e)
        {
            FeedDisplay.Items.Filter = target => ((XmlElement)target)["title"].InnerText.Contains(FilterBox.Text);

        }

        private void FeedDisplayHeader_OnClick(object sender, RoutedEventArgs e)
        {



            //((GridViewColumnHeader)e.OriginalSource).Column.Header.ToString() == "Datum"
            if (!(e.OriginalSource is GridViewColumnHeader ch)) return;
            var dir = ListSortDirection.Ascending;
            if (ch == lastHeaderClicked && lastDirection == ListSortDirection.Ascending)
                dir = ListSortDirection.Descending;
            SortColumnHeaderContent(ch, dir);
            lastHeaderClicked = ch; lastDirection = dir;


        }

        private void SortColumnHeaderContent(GridViewColumnHeader ch, ListSortDirection dir)
        {
            var bn = (ch.Column.DisplayMemberBinding as Binding)?.Path.Path;
            bn = bn ?? ch.Column.Header as string;
            var dv = CollectionViewSource.GetDefaultView(FeedDisplay.ItemsSource);
            dv.SortDescriptions.Clear();
            var sd = new SortDescription(bn, dir);
            dv.SortDescriptions.Add(sd);
            dv.Refresh();
        }


    }
}
