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
    /// Interaction logic for SelectCategoryDialog.xaml
    /// </summary>
    public partial class SelectCategoryDialog : Window
    {
        private List<CategoryRow> available_rows;
        private List<CategoryRow> datagrid_rows;
        public CategoryRow selected_row;
        public bool selected = false;
        public SelectCategoryDialog()
        {
            InitializeComponent();
            this.Set_rows();
        }

        private void Update_table()
        {
            SelectCategoryDialogDataGrid.ItemsSource = this.datagrid_rows;
        }
        private void Set_rows()
        {
            this.available_rows = DatabaseHandler.Get_categories(GlobalVariables.temporary_file_path); ;
            this.datagrid_rows = this.available_rows;
            this.Update_table();
        }
        private void Currency_selected(object sender, RoutedEventArgs e)
        {
            int selected_row_index = SelectCategoryDialogDataGrid.SelectedIndex;
            if (selected_row_index != -1)
            {
                this.selected_row = this.datagrid_rows[selected_row_index];
                this.selected = true;
                this.Close();
            }
        }

        private void Filter_currencies_by_name(object sender, RoutedEventArgs e)
        {
            this.datagrid_rows = this.available_rows.Where(row => row.Category_name.Contains(SelectCategoryDialogSearchQuery.Text)).ToList();
            this.Update_table();
        }
        private void Add_new_category(object sender, RoutedEventArgs e)
        {
            AddCategoryDialog add_category_dialog = new AddCategoryDialog();
            if (add_category_dialog.custom_show_dialog())
            {
                DatabaseHandler.Add_category(GlobalVariables.temporary_file_path, add_category_dialog.category_name);
                this.Set_rows();
            }
        }

        private void Delete_selected_rows(object sender, RoutedEventArgs e)
        {
            foreach (CategoryRow row in available_rows.Where(row => row.Selected == true).ToList())
            {
                DatabaseHandler.Delete_row_from_categories(GlobalVariables.temporary_file_path, row.Id);
            }
            this.Set_rows();
        }
        public bool custom_show_dialog()
        {
            this.ShowDialog();
            return this.selected;
        }
    }
}
