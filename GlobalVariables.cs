using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BasicAccounting
{
    public static class GlobalVariables
    {
        public static HomeView home_view = new HomeView();
        public static RegistryView registry_view;
        public static SearchView search_view;
        public static string current_file_path = "";
        public static string temporary_file_path = "";
        public static string current_view = "";
        public static CurrencyRow working_currency;
    }
}
