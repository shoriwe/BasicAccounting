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
    /// Interaction logic for InsertRegistryDialog.xaml
    /// </summary>
    public partial class ShowRegistryDialog : Window
    {
        private RegistryRow row;
        public bool success = false;
        public ShowRegistryDialog()
        {
            InitializeComponent();
        }

        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }
        private void Edit_this_row(object sender, RoutedEventArgs e)
        {
            EditRegistryDialog edit_registry_dialog = new EditRegistryDialog();
            edit_registry_dialog.Set_row(this.row);
            if (edit_registry_dialog.custom_show_dialog())
            {
                DatabaseHandler.Delete_row_from_registries(GlobalVariables.temporary_file_path, this.row.ID);
                this.Set_row(edit_registry_dialog.row);
                this.success = true;
            }
        }
        public void Set_row(RegistryRow row)
        {
            this.ShowRegistryDialogAmountField.Text = Math.Abs(row.Amount).ToString();
            this.ShowRegistryDialogCategoryField.Text = row.Category;
            this.ShowRegistryDialogCurrencyField.Text = row.Currency;
            this.ShowRegistryDialogDateBox.Text = row.Date.ToString();
            this.ShowRegistryDialogDescriptionField.Text = row.Description;
            if (row.Type_ == "income")
            {
                this.ShowRegistryDialogIncomeRadioButton.IsChecked = true;
            }
            else
            {
                this.ShowRegistryDialogOutcomeRadioButton.IsChecked = true;
            }
            this.row = row;
        }
    }
}
