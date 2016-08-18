using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UNotifier
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLink(object sender, MouseButtonEventArgs e)
        {
            Process.Start((sender as Control).Tag.ToString());
        }
    }
}
