using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for SelectCurrencyDialog.xaml
    /// </summary>
    public partial class SelectCurrencyDialog : Window
    {
        private List<CurrencyRow> available_rows;
        private List<CurrencyRow> datagrid_rows;
        private bool selected = false;

        public CurrencyRow selected_row;

        public SelectCurrencyDialog()
        {
            InitializeComponent();
            this.Set_rows();
        }
        public bool Custom_show_dialog()
        {
            this.ShowDialog();
            return this.selected;
        }

        private void Update_table()
        {
            SelectCurrencyDataGrid.ItemsSource = this.datagrid_rows;
        }
        private void Set_rows()
        {
            this.available_rows = DatabaseHandler.Get_currencies(GlobalVariables.temporary_file_path);
            this.datagrid_rows = this.available_rows;
            this.Update_table();
        }
        private void Currency_selected(object sender, RoutedEventArgs e)
        {
            int selected_row_index = SelectCurrencyDataGrid.SelectedIndex;
            if (selected_row_index != -1)
            {
                this.selected_row = this.datagrid_rows[selected_row_index];
                this.selected = true;
                this.Close();
            }
        }

        private void Filter_currencies_by_name(object sender, RoutedEventArgs e)
        {
            this.datagrid_rows = this.available_rows.Where(row => row.Currency_name.Contains(SelectCurrencyDialogTextBox.Text)).ToList();
            this.Update_table();
        }

        private void Add_new_currency(object sender, RoutedEventArgs e)
        {
            AddCurrencyDialog add_currency_dialog = new AddCurrencyDialog();
            if (add_currency_dialog.Custom_show_dialog())
            {
                this.Set_rows();
            }
        }
        private void Delete_selected_rows(object sender, RoutedEventArgs e)
        {
            foreach (CurrencyRow row in this.available_rows.Where(row => row.Selected == true).ToList())
            {
                if (row.Currency_name != "USD")
                {
                    DatabaseHandler.Delete_row_from_currencies(GlobalVariables.temporary_file_path, row.Id);
                }
                
            }
            this.Set_rows();
        }



    }
}
