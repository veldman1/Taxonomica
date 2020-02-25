using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Taxonomica.Common;
using Taxonomica.Common.JsonModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Taxonomica.Common.JsonModel.SearchResults;

namespace Taxonomica
{
    public sealed partial class SearchPage : Page
    {
        public static SearchResults SearchResultsList { get; set; }

        public SearchPage()
        {
            InitializeComponent();
            LoadingRing.IsActive = false;
            
            if (SearchResultsList == null)
            {
                SearchResultsList = new SearchResults();
            }

            SearchResults.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = SearchResultsList.MatchList, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Search();
        }

        private void SearchResults_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
            }
        }

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tsn = (((DataGrid)sender).SelectedItem as SearchResult).TSN;
            Frame.Navigate(typeof(TaxonPage), new TaxonPageNavigationArgs { TSN = tsn });
        }

        private void Search()
        {
            LoadingRing.IsActive = true;
            Entry.IsEnabled = false;
            ButtonSearch.IsEnabled = false;

            var searchTerm = Entry.Text;

            Task.Run(async () =>
            {
                SearchResultsList = new SearchResults();
                try
                {
                    SearchResultsList = await RequestManager.SearchByCommonName(searchTerm, 1);
                    foreach (var item in SearchResultsList.MatchList)
                    {
                        item.FirstCommonName = item.GetCommonName();
                    }
                }
                catch
                {
                }

                await DispatcherUtil.Dispatch(async () =>
                {
                    SearchResults.SetBinding(DataGrid.ItemsSourceProperty, new Binding { Source = SearchResultsList.MatchList, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                    LoadingRing.IsActive = false;
                    Entry.IsEnabled = true;
                    ButtonSearch.IsEnabled = true;
                });
            });
        }
    }
}
