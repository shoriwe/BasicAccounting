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
    /// Interaction logic for EditRegistryDialog.xaml
    /// </summary>
    public partial class EditRegistryDialog : Window
    {
        private bool success = false;
        private DateTime Date;
        private string Category;
        private string Type_;
        private double Amount;
        private string Description;
        private string Currency;
        public RegistryRow row;
        public EditRegistryDialog()
        {
            InitializeComponent();
        }
        public void Set_row(RegistryRow row)
        {
            this.EditRegistryDialogAmountBox.Text = Math.Abs(row.Amount).ToString();
            this.EditRegistryDialogCategoryButton.Content = row.Category;
            this.EditRegistryDialogCurrencyButton.Content = row.Currency;
            this.EditRegistryDialogDateBox.Text = row.Date.ToString();
            this.EditRegistryDialogDescriptionBox.Text = row.Description;
            if (row.Type_ == "income")
            {
                this.EditRegistryDialogIncomeRadioButton.IsChecked = true;
            }
            else
            {
                this.EditRegistryDialogOutcomeRadioButton.IsChecked = true;
            }
        }
            private bool Verify_date()
        {
            try
            {
                this.Date = Convert.ToDateTime(EditRegistryDialogDateBox.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool Verify_category()
        {
            string category = EditRegistryDialogCategoryButton.Content.ToString();
            if (DatabaseHandler.Sanitize_string(category))
            {
                if (DatabaseHandler.Category_exists(GlobalVariables.temporary_file_path, category))
                {
                    this.Category = category;
                    return true;
                }
            }
            else
            {
                // put here SQL injection detected dialog
            }
            return false;
        }

        private bool Verify_type()
        {
            if ((Convert.ToBoolean(EditRegistryDialogIncomeRadioButton.IsChecked) == true) ||
                (Convert.ToBoolean(EditRegistryDialogOutcomeRadioButton.IsChecked) == true))
            {
                if (EditRegistryDialogIncomeRadioButton.IsChecked == true)
                {
                    this.Type_ = "income";
                }
                else
                {
                    this.Type_ = "outcome";
                }
                return true;
            }
            return false;
        }
        private bool Verify_amount()
        {
            try
            {
                double amount = Convert.ToDouble(EditRegistryDialogAmountBox.Text);
                if (amount >= 0)
                {
                    this.Amount = amount;
                    return true;
                }
            }
            catch
            {
                
            }
            return false;
        }
        private bool Verify_description()
        {
            if (DatabaseHandler.Sanitize_string(EditRegistryDialogDescriptionBox.Text))
            {
                this.Description = EditRegistryDialogDescriptionBox.Text;
                return true;
            }
            return false;
        }
        private bool Verify_currency()
        {
            string currency = EditRegistryDialogCurrencyButton.Content.ToString();
            if (DatabaseHandler.Sanitize_string(currency))
            {
                if (DatabaseHandler.Currency_exists(GlobalVariables.temporary_file_path, currency))
                {
                    this.Currency = currency;
                    return true;
                }
            }
            else
            {
                // put here SQL injection detected dialog
            }
            return false;
        }
        private bool Verify_registry()
        {
            if (this.Verify_category() && this.Verify_date() && 
                this.Verify_type() && this.Verify_amount() && 
                this.Verify_description() && this.Verify_currency())
            {
                return true;
            }
            return false;
        }
        private void Insert_data_in_database(object sender, RoutedEventArgs e)
        {
            if (this.Verify_registry())
            {
                this.success = true;
                DatabaseHandler.Add_registry(GlobalVariables.temporary_file_path, this.Date, this.Category, this.Type_, this.Amount,  this.Description, this.Currency);
                this.row = new RegistryRow
                {
                    Date = this.Date,
                    Category = this.Category,
                    Type_ = this.Type_,
                    Amount = this.Amount,
                    Description = this.Description,
                    Currency = this.Currency
                };
                this.Close();
            }

        }
        private void Select_currency(object sender, RoutedEventArgs e)
        {
            SelectCurrencyDialog currency_selection_dialog = new SelectCurrencyDialog();
            if (currency_selection_dialog.Custom_show_dialog())
            {
                EditRegistryDialogCurrencyButton.Content = currency_selection_dialog.selected_row.Currency_name;
            }
        }
        private void Select_category(object sender, RoutedEventArgs e)
        {
            SelectCategoryDialog category_selection_dialog = new SelectCategoryDialog();
            if (category_selection_dialog.custom_show_dialog())
            {
                EditRegistryDialogCategoryButton.Content = category_selection_dialog.selected_row.Category_name;
            }
        }
        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }
    }
}
