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
    public partial class InsertRegistryDialog : Window
    {
        private bool success = false;
        private DateTime Date;
        private string Category;
        private string Type_;
        private double Amount;
        private string Description;
        private string Currency;
        public InsertRegistryDialog()
        {
            InitializeComponent();
        }

        private bool Verify_date()
        {
            try
            {
                this.Date = Convert.ToDateTime(InsertRegistryDialogDateBox.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool Verify_category()
        {
            string category = InsertRegistryDialogCategoryButton.Content.ToString();
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
            if ((Convert.ToBoolean(InsertRegistryDialogIncomeRadioButton.IsChecked) == true) ||
                (Convert.ToBoolean(InsertRegistryDialogOutcomeRadioButton.IsChecked) == true))
            {
                if (InsertRegistryDialogIncomeRadioButton.IsChecked == true)
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
                double amount = Convert.ToDouble(InsertRegistryDialogAmountBox.Text);
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
            if (DatabaseHandler.Sanitize_string(InsertRegistryDialogDescriptionBox.Text))
            {
                this.Description = InsertRegistryDialogDescriptionBox.Text;
                return true;
            }
            return false;
        }
        private bool Verify_currency()
        {
            string currency = InsertRegistryDialogCurrencyButton.Content.ToString();
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
                this.Close();
            }

        }
        private void Select_currency(object sender, RoutedEventArgs e)
        {
            SelectCurrencyDialog currency_selection_dialog = new SelectCurrencyDialog();
            if (currency_selection_dialog.Custom_show_dialog())
            {
                InsertRegistryDialogCurrencyButton.Content = currency_selection_dialog.selected_row.Currency_name;
            }
        }
        private void Select_category(object sender, RoutedEventArgs e)
        {
            SelectCategoryDialog category_selection_dialog = new SelectCategoryDialog();
            if (category_selection_dialog.custom_show_dialog())
            {
                InsertRegistryDialogCategoryButton.Content = category_selection_dialog.selected_row.Category_name;
            }
        }
        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }
    }
}
