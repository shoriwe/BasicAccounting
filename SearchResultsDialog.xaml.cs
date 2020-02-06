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
    /// Interaction logic for SearchResultsDialog.xaml
    /// </summary>
    public partial class SearchResultsDialog : Window
    {
        private List<RegistryRow> available_rows;
        private bool success = false;
        public SearchResultsDialog()
        {
            InitializeComponent();
        }
        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.success;
        }
        public void Set_rows(List<RegistryRow> rows)
        {
            this.available_rows = rows
            ;
            double summatory = 0;
            foreach (RegistryRow row in this.available_rows)
            {
                summatory += row.Amount * DatabaseHandler.Get_single_currency(GlobalVariables.temporary_file_path, row.Currency) / DatabaseHandler.Get_single_currency(GlobalVariables.temporary_file_path, SearchResultsCurrencyButton.Content.ToString());
            }
            SearchResultsSummatory.Text = summatory.ToString();
            SearchResultsDialogDataGrid.ItemsSource = this.available_rows;
        }
        private void Select_currency(object sender, RoutedEventArgs e)
        {
            SelectCurrencyDialog currency_selection_dialog = new SelectCurrencyDialog();
            if (currency_selection_dialog.Custom_show_dialog())
            {
                SearchResultsCurrencyButton.Content = currency_selection_dialog.selected_row.Currency_name;
                GlobalVariables.working_currency = currency_selection_dialog.selected_row;
                this.Set_rows(this.available_rows);
            }
        }

        private void Spawn_insert_registry_dialog(object sender, RoutedEventArgs e)
        {
            InsertRegistryDialog insertion_dialog = new InsertRegistryDialog();
            if (insertion_dialog.custom_show_dialog())
            {
                this.Set_rows(this.available_rows);
            }

        }
        private void Delete_selected_rows(object sender, RoutedEventArgs e)
        {
            foreach (RegistryRow row in available_rows.Where(row => row.Selected == true).ToList())
            {
                DatabaseHandler.Delete_row_from_registries(GlobalVariables.temporary_file_path, row.ID);
            }
            this.success = true;
            this.Set_rows(this.available_rows.Where(row => row.Selected == false).ToList());
        }

        private void Show_row(object sender, RoutedEventArgs e)
        {
            int selected_row_index = SearchResultsDialogDataGrid.SelectedIndex;
            if (selected_row_index != -1)
            {
                RegistryRow selected_row = this.available_rows[selected_row_index];
                ShowRegistryDialog regitry_show_row_dialog = new ShowRegistryDialog();
                regitry_show_row_dialog.Set_row(selected_row);
                if (regitry_show_row_dialog.custom_show_dialog())
                {
                    this.Set_rows(this.available_rows.Where(row => row.ID != selected_row.ID).ToList());
                    this.success = true;
                }
            }
        }
    }
}
