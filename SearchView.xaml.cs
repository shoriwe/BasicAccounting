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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : Page
    {
        public SearchView()
        {
            InitializeComponent();
        }
        private void Select_category(object sender, RoutedEventArgs e)
        {
            SelectCategoryDialog category_selection_dialog = new SelectCategoryDialog();
            if (category_selection_dialog.custom_show_dialog())
            {
                this.SearchCategoryButton.Content = category_selection_dialog.selected_row.Category_name;
            }
        }

        private bool Verify_category()
        {
            string category = SearchCategoryButton.Content.ToString();
            if (DatabaseHandler.Sanitize_string(category))
            {
                if (DatabaseHandler.Category_exists(GlobalVariables.temporary_file_path, category))
                {
                    return true;
                }
            }
            else
            {
                // put here SQL injection detected dialog
            }
            return false;
        }
        private void Basic_search(object sender, RoutedEventArgs e)
        {
            if (this.Verify_category())
            {
                RegistrySearchCondition condition = new RegistrySearchCondition
                {
                    Default_search = false,
                    Category = SearchCategoryButton.Content.ToString(),
                    Currency = "",
                    Amount_from = -1,
                    Amount_to = -1,
                    Type = "",
                    From_date = new DateTime(),
                    To_date = new DateTime(),
                    In_description = ""
                };
                List<RegistryRow> results = DatabaseHandler.Get_registries(GlobalVariables.temporary_file_path, condition);
                if (results.Count() >= 1)
                {
                    SearchResultsDialog results_dialog = new SearchResultsDialog();
                    results_dialog.Set_rows(results);
                    if (results_dialog.custom_show_dialog())
                    {
                        GlobalVariables.registry_view.Set_rows();
                    }
                } else
                {
                    NoResultsDialog no_results_dialog = new NoResultsDialog();
                    no_results_dialog.ShowDialog();
                }
            }
        }

        private void Advance_search(object sender, RoutedEventArgs e)
        {
            AdvanceSearchDialog advance_search_dialog = new AdvanceSearchDialog();
            if (advance_search_dialog.Custom_show_dialog())
            {
                GlobalVariables.registry_view.Set_rows();
            }
        }
    }
}
