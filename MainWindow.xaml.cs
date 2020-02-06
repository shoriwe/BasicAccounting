using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;


namespace BasicAccounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (GlobalVariables.current_view == "" && GlobalVariables.current_file_path == "")
            {
                ViewPlaceHolder.Content = GlobalVariables.home_view;
            }
        }

        private void Go_to_registry_view(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.current_view != "regitry" && GlobalVariables.current_file_path != "") {
                ViewPlaceHolder.Content = GlobalVariables.registry_view;
                GlobalVariables.current_view = "registry";
            }
        }

        private void Go_to_search_view(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.current_view != "search" && GlobalVariables.current_file_path != "")
            {
                ViewPlaceHolder.Content = GlobalVariables.search_view;
                GlobalVariables.current_view = "search";
            }
        }

        private void Init_registry_view()
        {
            GlobalVariables.registry_view = new RegistryView();
            GlobalVariables.search_view = new SearchView();

            // Load the registry view
            ViewPlaceHolder.Content = GlobalVariables.registry_view;
            GlobalVariables.current_view = "registry";
        }
        public void Create_new_file(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file_dialog = new SaveFileDialog();
            file_dialog.DefaultExt = "db";
            file_dialog.FileName = "my_accounting";
            if (file_dialog.ShowDialog() == true)
            {
                if (File.Exists(file_dialog.FileName))
                {
                    File.Delete(file_dialog.FileName);
                }
                FileHandler.New_file(file_dialog.FileName);

                this.Init_registry_view();
                this.Save_file(sender, e);
            }
        }

        public void Open_existing_file(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.DefaultExt = "db";
            if (file_dialog.ShowDialog() == true)
            {
                FileHandler.Open_file(file_dialog.FileName);

                this.Init_registry_view();
            }
        }

        private void Save_file(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.current_file_path != "" && GlobalVariables.temporary_file_path != "")
            {
                FileHandler.Save_file(GlobalVariables.current_file_path, GlobalVariables.temporary_file_path);
            }
        }

        private void Save_file_as(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.current_file_path != "" && GlobalVariables.temporary_file_path != "")
            {
                SaveFileDialog save_as_dialog = new SaveFileDialog();
                if (save_as_dialog.ShowDialog() == true)
                {
                    GlobalVariables.current_file_path = save_as_dialog.FileName;
                    this.Save_file(sender, e);
                }
            }
        }
        private void Exit_program(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
