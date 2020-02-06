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
using System.IO;
using Microsoft.Win32;

namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    /// 
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent();
        }
        private void Create_new_file(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).Create_new_file(sender, e);
        }
        private void Open_existing_file(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).Open_existing_file(sender, e);
        }
    }
}
