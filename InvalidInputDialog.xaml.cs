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
using System.Windows.Shapes;

namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for InvalidInputDialog.xaml
    /// </summary>
    public partial class InvalidInputDialog : Window
    {
        public InvalidInputDialog()
        {
            InitializeComponent();
        }

        public void Set_msg(string msg)
        {
            this.InvalidInputDialogTexBlock.Text = msg;
        }
        private void Ok_button_pressed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
