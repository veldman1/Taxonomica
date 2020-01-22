using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxonomica.Common;
using Windows.UI.Xaml;

namespace Taxonomica
{
    public class SynonymItem : DependencyObject
    {
        public SynonymItem(TaxonRecordSynonym trs)
        {
            TSN = trs.TSN;
            SciName = trs.SciName;
            if (string.IsNullOrWhiteSpace(trs.SciName))
            {
                SciName = "---";
            }
        }

        public static readonly DependencyProperty TSNProperty =
                        DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(SynonymItem),
            null);

        public string TSN
        {
            get { return (string)GetValue(TSNProperty); }
            set { SetValue(TSNProperty, (string)value); }
        }

        public static readonly DependencyProperty SciNameProperty =
                        DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(SynonymItem),
            null);

        public string SciName
        {
            get { return (string)GetValue(SciNameProperty); }
            set { SetValue(SciNameProperty, (string)value); }
        }
    }
}
