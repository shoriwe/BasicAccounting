using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlTypes;


namespace BasicAccounting
{
    public static class DatabaseHandler
    {

        public static bool Sanitize_string(string string_)
        {
            foreach (char dangerous_char in "!@#$%^&*()_+-=`~'\"\\|}{[]<>,./?")
            {
                if (string_.Contains(dangerous_char))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Category_exists(string file_path, string category_name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM categories WHERE category = (@param1);"
            };
            command.Parameters.Add("@param1", System.Data.DbType.String).Value = category_name;
            SQLiteDataReader results_reader = command.ExecuteReader();
            while (results_reader.Read())
            {
                if (Convert.ToString(results_reader["category"]) == category_name)
                {
                    results_reader.Close();
                    connection.Close();
                    return true;
                }
            }
            results_reader.Close();
            connection.Close();
            return false;
        }

        public static bool Currency_exists(string file_path, string currency_name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM currencies WHERE currency = (@param1);"
            };
            command.Parameters.Add("@param1", System.Data.DbType.String).Value = currency_name;
            SQLiteDataReader results_reader = command.ExecuteReader();
            while (results_reader.Read())
            {
                if (Convert.ToString(results_reader["currency"]) == currency_name)
                {
                    results_reader.Close();
                    connection.Close();
                    return true;
                }
            }
            results_reader.Close();
            connection.Close();
            return false;
        }

        public static void Create_database(string file_path)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();

            // Create registries table
            SQLiteCommand registries_command = new SQLiteCommand(connection)
            {
                CommandText = "CREATE TABLE IF NOT EXISTS registries (id INTEGER PRIMARY KEY AUTOINCREMENT, date TEXT, category TEXT, type TEXT, amount REAL, description TEXT, currency TEXT);"
            };
            registries_command.ExecuteNonQuery();
            // Create categories table
            SQLiteCommand categories_command = new SQLiteCommand(connection)
            {
                CommandText = "CREATE TABLE IF NOT EXISTS categories (id INTEGER PRIMARY KEY AUTOINCREMENT, category TEXT);"
            };
            categories_command.ExecuteNonQuery();
            // Create categories table
            SQLiteCommand currencies_command = new SQLiteCommand(connection)
            {
                CommandText = "CREATE TABLE IF NOT EXISTS currencies (id INTEGER PRIMARY KEY AUTOINCREMENT, currency TEXT, vs_usd_rate REAL);"
            };
            currencies_command.ExecuteNonQuery();
            connection.Close();
            DatabaseHandler.Add_currency(file_path, "USD", 1);
        }
        
        public static double Get_single_currency(string file_path, string currency_name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM currencies"
            };
            SQLiteDataReader results_reader = command.ExecuteReader();
            while (results_reader.Read())
            {
                if (Convert.ToString(results_reader["currency"]) == currency_name)
                {
                    return Convert.ToDouble(results_reader["vs_usd_rate"]);
                }
            }
            results_reader.Close();
            connection.Close();
            return 1;
        }
        public static void Add_currency(string file_path, string currency, double vs_usd_rate)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand add_command = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO currencies (currency, vs_usd_rate) VALUES (@param1, @param2)"
            };
            add_command.Parameters.Add("@param1", System.Data.DbType.String).Value = currency;
            add_command.Parameters.Add("@param2", System.Data.DbType.Double).Value = vs_usd_rate;
            add_command.ExecuteNonQuery();
            connection.Close();
        }
        public static void Add_category(string file_path, string category_name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand add_command = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO categories (category) VALUES (@param1)"
            };
            add_command.Parameters.Add("@param1", System.Data.DbType.String).Value = category_name;
            add_command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Add_registry(string file_path, DateTime date, string category, string type, double amount, string description, string currency)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand add_command = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO registries (date, category, type, amount, description, currency) VALUES (@param1, @param2, @param3, @param4, @param5, @param6)"
            };
            add_command.Parameters.Add("@param1", System.Data.DbType.DateTime).Value = date;
            add_command.Parameters.Add("@param2", System.Data.DbType.String).Value = category;
            add_command.Parameters.Add("@param3", System.Data.DbType.String).Value = type;
            add_command.Parameters.Add("@param4", System.Data.DbType.Double).Value = amount;
            add_command.Parameters.Add("@param5", System.Data.DbType.String).Value = description;
            add_command.Parameters.Add("@param6", System.Data.DbType.String).Value = currency;
            add_command.ExecuteNonQuery();
            connection.Close();
        }
        public static List<CurrencyRow> Get_currencies(string file_path)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM currencies"
            };
            SQLiteDataReader results_reader = command.ExecuteReader();
            List<CurrencyRow> rows = new List<CurrencyRow>();
            while (results_reader.Read())
            {
                CurrencyRow row = new CurrencyRow
                {
                    Id = Convert.ToInt32(results_reader["id"]),
                    Currency_name = Convert.ToString(results_reader["currency"]),
                    Vs_usd_rate = Convert.ToDouble(results_reader["vs_usd_rate"])
                };
                rows.Add(row);
            }
            results_reader.Close();
            connection.Close();
            return rows;
        }

        public static List<CategoryRow> Get_categories(string file_path)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM categories;"
            };
            SQLiteDataReader results_reader = command.ExecuteReader();
            List<CategoryRow> rows = new List<CategoryRow>();
            while (results_reader.Read())
            {
                CategoryRow row = new CategoryRow
                {
                    Id = Convert.ToInt32(results_reader["id"]),
                    Category_name = Convert.ToString(results_reader["category"])
                };
                rows.Add(row);
            }
            results_reader.Close();
            connection.Close();
            return rows;
        }

        public static void Delete_row_from_currencies(string file_path, int id)
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM currencies WHERE id=(@param1)"
            };
            command.Parameters.Add("@param1", System.Data.DbType.Int32).Value = id;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static void Delete_row_from_registries(string file_path, int id)
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM registries WHERE id=(@param1)"
            };
            command.Parameters.Add("@param1", System.Data.DbType.Int32).Value = id;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Delete_row_from_categories(string file_path, int id)
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM categories WHERE id=(@param1)"
            };
            command.Parameters.Add("@param1", System.Data.DbType.Int32).Value = id;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static List<RegistryRow> Get_registries(string file_path, RegistrySearchCondition condition)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("DataSource={0}", file_path));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM registries"
            };
            SQLiteDataReader results_reader = command.ExecuteReader();
            List<RegistryRow> rows = new List<RegistryRow>();
            while (results_reader.Read())
            {
                RegistryRow row = new RegistryRow
                {
                    ID = Convert.ToInt32(results_reader["id"]),
                    Amount = Convert.ToDouble(results_reader["amount"]),
                    Date = Convert.ToDateTime(results_reader["date"]),
                    Category = Convert.ToString(results_reader["category"]),
                    Type_ = Convert.ToString(results_reader["type"]),
                    Description = Convert.ToString(results_reader["description"]),
                    Currency = Convert.ToString(results_reader["currency"])
                };
                if (condition.Default_search)
                {
                    if (row.Type_ == "outcome")
                    {
                        row.Amount *= -1;
                    }
                    rows.Add(row);
                } else
                {
                    if (condition.Check_condition(row.Date, row.Category, row.Type_, row.Amount, row.Description, row.Currency))
                    {
                        if (row.Type_ == "outcome")
                        {
                            row.Amount *= -1;
                        }
                        rows.Add(row);
                    }
                }
            }
            results_reader.Close();
            connection.Close();
            return rows;
        }
        
    }
}
