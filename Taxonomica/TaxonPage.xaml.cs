using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Taxonomica.Common;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaxonPage : Page
    {
        public TaxonPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var args = (TaxonPageNavigationArgs)e.Parameter;

            ProgressItem.IsActive = true;
            Task.Run(async () =>
            {
                try
                {
                    if (args == null)
                    {
                        await LoadKingdoms();
                    }
                    else
                    {
                        await LoadTSN(args.TSN);
                    }
                }
                catch
                {
                    await DispatcherUtil.Dispatch(async () =>
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "The page could not be loaded",
                            CloseButtonText = "Ok",
                        };

                        ContentDialogResult result = await errorDialog.ShowAsync();
                    });
                }
                finally
                {
                    await DispatcherUtil.Dispatch(() =>
                    {
                        ProgressItem.IsActive = false;
                    });
                }
            });
        }

        private async Task LoadTSN(string tsn)
        {
            var hierarchy = await RequestManager.RequestFullHierarchy(tsn);
            var currentTaxonHierarchy = hierarchy.HierarchyList.Where(x => x.TSN.Equals(tsn)).FirstOrDefault();

            var currentTaxon = await RequestManager.RequestFullRecord(tsn);

            var image = await RequestManager.RequestWikispeciesImage(currentTaxonHierarchy.TaxonName);
            var imageSource = image.Query.Pages.FirstOrDefault().Value.Thumbnail?.Source;

            var childRankName = Rank.Next(currentTaxonHierarchy.RankName);
            var children = new List<HierarchyItem>();
            while (children.Count == 0 && childRankName != null)
            {
                children = hierarchy.HierarchyList.Where(hierarchyItem => hierarchyItem.RankName.Equals(childRankName)).ToList();
                childRankName = Rank.Next(childRankName);
            }

            await DispatcherUtil.Dispatch(() =>
            {
                TheList.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = children });
                TaxonName.Text = currentTaxonHierarchy.TaxonName;

                CommonName.Text = currentTaxon.GetCommonName();

                RankName.Text = currentTaxonHierarchy.RankName;

                if (imageSource != null)
                {
                    TaxonImage.Source = new BitmapImage(new Uri(imageSource, UriKind.Absolute));
                }
            });
        }

        private async Task LoadKingdoms()
        {
            var kingdoms = new List<HierarchyItem>()
            {
                            new HierarchyItem { TSN = "202423", TaxonName = "Animalia", RankName = "Kingdom" },
                            new HierarchyItem { TSN = "202422", TaxonName = "Plantae", RankName = "Kingdom" },
                            new HierarchyItem { TSN = "555705", TaxonName = "Fungi", RankName = "Kingdom" },
                            new HierarchyItem { TSN = "630577", TaxonName = "Fungi", RankName = "Kingdom" },
                            new HierarchyItem { TSN = "630578", TaxonName = "Chromista", RankName = "Kingdom" },
                            new HierarchyItem { TSN = "935939", TaxonName = "Archaea", RankName = "Kingdom" },
            };

            await DispatcherUtil.Dispatch(() =>
            {
                TheList.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = kingdoms });
            });
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var tsn = (((StackPanel)sender).DataContext as HierarchyItem).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });
        }
    }
}
