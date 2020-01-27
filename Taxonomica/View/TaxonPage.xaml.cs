using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        await LoadRoot();
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
            var ascending = new List<HierarchyItem>();

            children = hierarchy.HierarchyList.Where(hierarchyItem => Rank.NumRankOf(childRankName) <= Rank.NumRankOf(hierarchyItem.RankName)).ToList();
            ascending = hierarchy.HierarchyList.Where(hierarchyItem => Rank.NumRankOf(childRankName) > Rank.NumRankOf(hierarchyItem.RankName)).ToList();

            await DispatcherUtil.Dispatch(() =>
            {
                TheList.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = children });
                if (!children.Any())
                {
                    DescendingGrid.Visibility = Visibility.Collapsed;
                }

                TaxonName.Text = currentTaxonHierarchy.TaxonName;
                CommonName.Text = currentTaxon.GetCommonName();
                RankName.Text = currentTaxonHierarchy.RankName;

                if (string.IsNullOrWhiteSpace(currentTaxon.Author.Author))
                {
                    AuthorshipEntry.Visibility = Visibility.Collapsed;
                }
                else
                {
                    AuthorshipEntry.Visibility = Visibility.Visible;
                    AuthorName.Text = currentTaxon.Author.Author;
                }

                if (imageSource != null)
                {
                    TaxonImage.Source = new BitmapImage(new Uri(imageSource, UriKind.Absolute));
                }

                var synonyms = currentTaxon.SynonymList.Synonyms
                    .Where(x => x != null)
                    .Select(x => new SynonymItem(x));

                var synonymsCollection = new ObservableCollection<SynonymItem>(synonyms);

                if (synonymsCollection.Count == 0)
                {
                    SynonymsList.Visibility = Visibility.Collapsed;
                    SynonymsButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SynonymsList.Visibility = Visibility.Visible;
                    SynonymsButton.Visibility = Visibility.Visible;
                    SynonymsList.SetBinding(ListBox.ItemsSourceProperty, new Binding { Source = synonymsCollection });
                }

                var pathItems = ascending.OrderBy(x => Rank.Ranks.ToList().IndexOf(x.RankName)).ToList();
                TaxonPath.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = pathItems });
            });
        }

        private async Task LoadRoot()
        {
            var kingdoms = new List<HierarchyItem>()
            {
                new HierarchyItem { TSN = "202423", TaxonName = "Animalia", RankName = "Kingdom" },
                new HierarchyItem { TSN = "202422", TaxonName = "Plantae", RankName = "Kingdom" },
                new HierarchyItem { TSN = "555705", TaxonName = "Fungi", RankName = "Kingdom" },
                new HierarchyItem { TSN = "630577", TaxonName = "Protozoa", RankName = "Kingdom" },
                new HierarchyItem { TSN = "630578", TaxonName = "Chromista", RankName = "Kingdom" },
                new HierarchyItem { TSN = "935939", TaxonName = "Archaea", RankName = "Kingdom" },
            };

            await DispatcherUtil.Dispatch(() =>
            {
                TheList.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = kingdoms });
                TaxonName.Text = "Welcome to Taxonomica!";
                RankName.Text = "Select a kingdom to get started";
            });
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var tsn = (((StackPanel)sender).DataContext as HierarchyItem).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });
        }

        private void StackPanel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            var tsn = (((StackPanel)sender).DataContext as SynonymItem).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });
        }

        private void StackPanel_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            var tsn = (((StackPanel)sender).DataContext as HierarchyItem).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });

        }
    }
}
