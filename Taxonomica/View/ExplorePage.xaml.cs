using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Taxonomica.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Taxonomica
{
    public sealed partial class ExplorePage : Page
    {
        public ExplorePage()
        {
            InitializeComponent();

            try
            {
                Task.Run(InitializeExplorePage);
            }
            catch
            {
            }
        }

        private async Task InitializeExplorePage()
        {
            await DispatcherUtil.Dispatch(async () =>
                {
                    var exploreItems = new ObservableCollection<ExploreItem>();

                    try
                    {
                        var tsnList = new List<string> { "174371", "173420", "161061", "159785", "500028", "180130", "552304", "552303" };

                        ExploreGrid.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = exploreItems });

                        foreach (var tsn in tsnList)
                        {
                            var newItem = new ExploreItem();

                            newItem.Loading = true;

                            var record = await RequestManager.RequestFullRecord(tsn);

                            newItem.Name = record.GetCommonName();
                            newItem.TSN = record.ScientificName.TSN;

                            // var imageModelLow = await RequestManager.RequestWikispeciesImage(record.ScientificName.CombinedName, 50);
                            // newItem.Image = new BitmapImage(new Uri(imageModelLow.GetThumbnail(), UriKind.Absolute));

                            var imageModel = await RequestManager.RequestWikispeciesImage(record.ScientificName.CombinedName, 400);
                            newItem.Image = new BitmapImage(new Uri(imageModel.GetThumbnail(), UriKind.Absolute));

                            newItem.Loading = false;

                            exploreItems.Add(newItem);
                        }
                    }
                    catch (Exception e)
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = e.Message + " " + e.InnerException.ToString(),
                            CloseButtonText = "Ok",
                        };
                        ContentDialogResult result = await errorDialog.ShowAsync();

                        exploreItems.Clear();

                        Frame.Navigate(typeof(ErrorPage), null);
                    }
                });

        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var tsn = (((Grid)sender).DataContext as ExploreItem).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });
        }
    }
}
