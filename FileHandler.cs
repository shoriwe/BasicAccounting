using System.IO;


namespace BasicAccounting
{
    public static class FileHandler
    {
        public static void New_file(string file_path)
        {
            GlobalVariables.current_file_path = file_path;
            GlobalVariables.temporary_file_path = Path.GetTempFileName();
            DatabaseHandler.Create_database(file_path);
            File.Copy(file_path, GlobalVariables.temporary_file_path, true);
        }
        public static void Open_file(string file_path)
        {
            GlobalVariables.current_file_path = file_path;
            GlobalVariables.temporary_file_path = Path.GetTempFileName();
            File.Copy(file_path, GlobalVariables.temporary_file_path, true);
        }
        public static void Save_file(string file_path, string tmp_file_path)
        {
            File.Copy(tmp_file_path, file_path, true);
        }
        public static void Save_as_file(string file_path, string tmp_file_path)
        {

        }
    }
}
