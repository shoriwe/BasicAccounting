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
    /// Interaction logic for AdvanceSearchDialog.xaml
    /// </summary>
    public partial class AdvanceSearchDialog : Window
    {
        private bool success = false;
        private string Type_;
        private string Description;
        private string Category;
        private string Currency;
        private DateTime Date_to;
        private DateTime Date_from;
        private Double Amount_from;
        private Double Amount_to;
        public AdvanceSearchDialog()
        {
            InitializeComponent();
        }

        public bool Custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }

        private bool Verify_date_from()
        {
            if (AdvanceSearchDialogDateFrom.Text == "")
            {
                return true;
            }
            try
            {
                this.Date_from = Convert.ToDateTime(AdvanceSearchDialogDateFrom.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool Verify_date_to()
        {
            if (AdvanceSearchDialogDateTo.Text == "")
            {
                return true;
            }
            try
            {
                this.Date_to = Convert.ToDateTime(AdvanceSearchDialogDateTo.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool Verify_category()
        {
            if (AdvanceSearchDialogCategoryButton.Content.ToString() == "Category")
            {
                this.Category = "";
                return true;
            }
            string category = AdvanceSearchDialogCategoryButton.Content.ToString();
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
            if ((Convert.ToBoolean(AdvaceSearchDialogIncomeRadio.IsChecked) == true) ||
                (Convert.ToBoolean(AdvaceSearchDialogOutcomeRadio.IsChecked) == true))
            {
                if (AdvaceSearchDialogIncomeRadio.IsChecked == true)
                {
                    this.Type_ = "income";
                }
                else
                {
                    this.Type_ = "outcome";
                }
                return true;
            } else
            {
                this.Type_ = "";
                return true;
            }
        }
        private bool Verify_amount_from()
        {

            if (AdvanceSearchDialogAmountFrom.Text == "")
            {
                this.Amount_from = -1;
                return true;
            }
            try
            {
                double amount = Convert.ToDouble(AdvanceSearchDialogAmountFrom.Text);
                if (amount >= 0)
                {
                    this.Amount_from = amount;
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        private bool Verify_amount_to()
        {
            if (AdvanceSearchDialogAmountTo.Text == "")
            {
                this.Amount_to = -1;
                return true;
            }
            try
            {
                double amount = Convert.ToDouble(AdvanceSearchDialogAmountTo.Text);
                if (amount >= 0)
                {
                    this.Amount_to = amount;
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
            if (AdvanceSearchDialogDescriptionBox.Text == "")
            {
                this.Description = "";
                return true;
            }
            if (DatabaseHandler.Sanitize_string(AdvanceSearchDialogDescriptionBox.Text))
            {
                this.Description = AdvanceSearchDialogDescriptionBox.Text;
                return true;
            }
            return false;
        }
        private bool Verify_currency()
        {
            if (AdvanceSearchDialogCurrencyButton.Content.ToString() == "Currency")
            {
                this.Currency = "";
                return true;
            }
            string currency = AdvanceSearchDialogCurrencyButton.Content.ToString();
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
        private bool Verify_search()
        {
            if (this.Verify_category() && this.Verify_date_from() && this.Verify_date_to() &&
                this.Verify_type() && this.Verify_amount_from() && this.Verify_amount_to() &&
                this.Verify_description() && this.Verify_currency())
            {
                return true;
            }
            return false;
        }

        private void Select_category(object sender, RoutedEventArgs e)
        {
            SelectCategoryDialog category_selection_dialog = new SelectCategoryDialog();
            if (category_selection_dialog.custom_show_dialog())
            {
                this.AdvanceSearchDialogCategoryButton.Content = category_selection_dialog.selected_row.Category_name;
            }
        }

        private void Select_currency(object sender, RoutedEventArgs e)
        {
            SelectCurrencyDialog category_selection_dialog = new SelectCurrencyDialog();
            if (category_selection_dialog.Custom_show_dialog())
            {
                this.AdvanceSearchDialogCurrencyButton.Content = category_selection_dialog.selected_row.Currency_name;
            }
        }
        private void Search_button(object sender, RoutedEventArgs e)
        {
            if (this.Verify_search())
            {
                RegistrySearchCondition condition = new RegistrySearchCondition
                {
                    Type = this.Type_,
                    In_description = this.Description,
                    Category = this.Category,
                    Currency = this.Currency,
                    To_date = this.Date_to,
                    From_date = this.Date_from,
                    Amount_from = this.Amount_from,
                    Amount_to = this.Amount_to
                };
                List<RegistryRow> search_results = DatabaseHandler.Get_registries(GlobalVariables.temporary_file_path, condition);
                if (search_results.Count() >= 1)
                {
                    SearchResultsDialog results_dialog = new SearchResultsDialog();
                    results_dialog.Set_rows(search_results);
                    if (results_dialog.custom_show_dialog())
                    {
                        this.success = true;
                    }
                } else
                {
                    NoResultsDialog no_results_dialog = new NoResultsDialog();
                    no_results_dialog.ShowDialog();
                }
            }
        }
        
    }
}
