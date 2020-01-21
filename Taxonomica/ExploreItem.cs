using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Taxonomica
{
    internal class ExploreItem : DependencyObject
    {
        public static readonly DependencyProperty ImageProperty =
                DependencyProperty.Register(
                    "Image",
                    typeof(BitmapImage),
                    typeof(ExploreItem),
                    null);

        public static readonly DependencyProperty LoadingProperty =
                DependencyProperty.Register(
                    "Loading",
                    typeof(bool),
                    typeof(ExploreItem),
                    null);

        public static readonly DependencyProperty NameProperty =
                                DependencyProperty.Register(
                    "Name",
                    typeof(string),
                    typeof(ExploreItem),
                    null);

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, (BitmapImage)value); }
        }

        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, (bool)value); }
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, (string)value); }
        }
    }
}