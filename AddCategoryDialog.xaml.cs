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
    /// Interaction logic for AddCategoryDialog.xaml
    /// </summary>
    public partial class AddCategoryDialog : Window
    {
        private bool success= false;
        public string category_name;
        public AddCategoryDialog()
        {
            InitializeComponent();
        }

        private void Add_new_category(object sender, RoutedEventArgs e)
        {
            string category_name = AddCategoryDialogCategoryName.Text;
            if (DatabaseHandler.Sanitize_string(category_name))
            {
                if (category_name == "Category"){
                    InvalidInputDialog invalid_input_dialog = new InvalidInputDialog();
                    invalid_input_dialog.Set_msg("Invalid category name");
                    invalid_input_dialog.ShowDialog();
                    return;
                }
                if (DatabaseHandler.Category_exists(GlobalVariables.temporary_file_path, category_name) != true)
                {
                    this.category_name = category_name;
                    this.success = true;
                    this.Close();
                }
            } else
            {
                InvalidInputDialog invalid_input_dialog = new InvalidInputDialog();
                invalid_input_dialog.Set_msg("Are you using any ilegal chars ('!@#$%^&*()_+=,./\";:[]{}\\|)?");
                invalid_input_dialog.ShowDialog();
            }
        }
        private void Cancel_add_category(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }
    }
}
