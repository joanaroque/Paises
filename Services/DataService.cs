namespace Paises.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;

        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists countries " +
                    "(Name varchar(50), Capital varchar(50), " +
                    "Region varchar(50), " +
                    "Subregion varchar(50), Population int," +
                    "Gini real, Flag varchar(200))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Erro", ex.Message);
            }
        }
        public void SaveData(List<RootObject> Countries)
        {
            try
            {
                foreach (var country in Countries)
                {
                    string sql = string.Format("insert into Rates (Name, Capital, Region, Subregion, Population, Gini, Flag)" +
                        " values('{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}')", country.Name, country.Capital, country.Region, country.Subregion,
                        country.Population, country.Gini, country.Flag);

                    command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Erro", ex.Message);
            }
        }
        public List<RootObject> GetData()
        {
            List<RootObject> countries = new List<RootObject>();

            try
            {
                string sql = "select Name, Capital, Region, Subregion, Population, Gini, Flag from Countries";

                command = new SQLiteCommand(sql, connection);

                // lê cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())//enquanto tiver registos pra ler
                {
                    countries.Add(new RootObject
                    {
                        Name = (string)reader["Name"],
                        Capital = (string)reader["Capital"],
                        Region = (string)reader["Region"],
                        Subregion = (string)reader["Subregion"],
                        Population = (int)reader["Population"],
                        Gini = (double)reader["Gini"],
                        Flag = (string)reader["Flag"]
                    });
                }

                connection.Close();

                return countries;
            }

            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }
        public void DeleteData()
        {
            try
            {
                string sql = "delete from Countries";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
