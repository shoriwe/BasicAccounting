﻿using System;
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
    /// Interaction logic for RegistryView.xaml
    /// </summary>
    public partial class RegistryView : Page
    {
        private List<RegistryRow> available_rows;
        public RegistryView()
        {
            InitializeComponent();
            this.Set_rows();
        }

        public void Set_rows()
        {
            this.available_rows = DatabaseHandler.Get_registries(GlobalVariables.temporary_file_path, new RegistrySearchCondition { Default_search = true });
            ;
            double summatory = 0;
            foreach (RegistryRow row in this.available_rows)
            {
                summatory += row.Amount * DatabaseHandler.Get_single_currency(GlobalVariables.temporary_file_path, row.Currency) / DatabaseHandler.Get_single_currency(GlobalVariables.temporary_file_path, RegistryCurrencyButton.Content.ToString());
            }
            RegistrySumatory.Text = summatory.ToString();
            RegistryViewDataGrid.ItemsSource = this.available_rows;
        }
        private void Select_currency(object sender, RoutedEventArgs e)
        {
            SelectCurrencyDialog currency_selection_dialog = new SelectCurrencyDialog();
            if (currency_selection_dialog.Custom_show_dialog())
            {
                RegistryCurrencyButton.Content = currency_selection_dialog.selected_row.Currency_name;
                GlobalVariables.working_currency = currency_selection_dialog.selected_row;
                this.Set_rows();
            }
        }

        private void Spawn_insert_registry_dialog(object sender, RoutedEventArgs e)
        {
            InsertRegistryDialog insertion_dialog = new InsertRegistryDialog();
            if (insertion_dialog.custom_show_dialog())
            {
                this.Set_rows();
            }

        }
        private void Delete_selected_rows(object sender, RoutedEventArgs e)
        {
            foreach (RegistryRow row in available_rows.Where(row => row.Selected == true).ToList())
            {
                DatabaseHandler.Delete_row_from_registries(GlobalVariables.temporary_file_path, row.ID);
            }
            this.Set_rows();
        }

        private void Show_row(object sender, RoutedEventArgs e)
        {
            int selected_row_index = RegistryViewDataGrid.SelectedIndex;
            if (selected_row_index != -1)
            {
                RegistryRow selected_row = this.available_rows[selected_row_index];
                ShowRegistryDialog regitry_show_row_dialog = new ShowRegistryDialog();
                regitry_show_row_dialog.Set_row(selected_row);
                if (regitry_show_row_dialog.custom_show_dialog())
                {
                    this.Set_rows();
                }
            }
        }
    }
}

