using System;
using System.Windows;


namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for AddCurrencyDialog.xaml
    /// </summary>
    public partial class AddCurrencyDialog : Window
    {
        private bool correcly_created = false;
        public AddCurrencyDialog()
        {
            InitializeComponent();
        }
        private void Cancel_add_currency(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add_new_currency(object sender, RoutedEventArgs e)
        {
            string currency_name = AddCurrencyDialogCurrencyNameBox.Text;
            if (DatabaseHandler.Sanitize_string(currency_name))
            {
                if (DatabaseHandler.Currency_exists(GlobalVariables.temporary_file_path, currency_name) != true)
                {
                    double vs_usd_rate;
                    try
                    {
                        vs_usd_rate = Convert.ToDouble(AddCurrencyDialogCurrencyRateBox.Text);
                    }
                    catch
                    {
                        return;
                    }
                    if (vs_usd_rate > 0)
                    {
                        DatabaseHandler.Add_currency(GlobalVariables.temporary_file_path, currency_name, vs_usd_rate);
                        this.correcly_created = true;
                        this.Close();
                    } else
                    {
                        InvalidInputDialog invalid_input_dialog = new InvalidInputDialog();
                        invalid_input_dialog.Set_msg("The Currency rate can't be negative");
                        invalid_input_dialog.ShowDialog();
                    }
                }
            } else
            {
                InvalidInputDialog invalid_input_dialog = new InvalidInputDialog();
                invalid_input_dialog.Set_msg("Are you using any ilegal chars ('!@#$%^&*()_+=,./\";:[]{}\\|)?");
                invalid_input_dialog.ShowDialog();
            }
        }
        public bool Custom_show_dialog()
        {
            this.ShowDialog();
            return this.correcly_created;
        }
    }
}
