using System.Windows;
using System.Windows.Media;

namespace UNotifier
{
    public abstract class ViewModel
    {
        public static SolidColorBrush New = Application.Current.FindResource("NewEntryBrush") as SolidColorBrush,
                                      Watched = Application.Current.FindResource("WatchedEntryBrush") as SolidColorBrush;

        public abstract void RefreshData();

        public ViewModel()
        {
            RefreshData();
        }
    }
}
