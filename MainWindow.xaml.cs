using RSS_Reader.Components;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

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
                if(Path == null)
                {
                    MessageBox.Show("Bitte klicken sie auf einen Eintrag zum öffnen", "Kein Eintrag Ausgewählt", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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
            GridViewColumnHeader? headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    this.SortByColumnHeader(sortBy, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }


        }

        private void SortByColumnHeader(string sortBy, ListSortDirection direction)
        {
            ListCollectionView dataView =
              (ListCollectionView)CollectionViewSource.GetDefaultView(FeedDisplay.ItemsSource);
            dataView.SortDescriptions.Clear();
            if (String.Equals(sortBy, "Datum"))
            {
                dataView.CustomSort = new DateTimeComparer(direction);
            }
            else if (String.Equals(sortBy, "Artikel")){
                dataView.SortDescriptions.Add(new SortDescription("title", direction));
            }
            dataView.Refresh();
        }


    }
}
