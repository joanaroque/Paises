namespace Countries.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.IO;
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;

        public  DataService()
        {
            dialogService = new DialogService();// TODO tirar do construtor e por noutro metodo

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");// todo acrescentar metodos para a outra api
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
        public void CreateData()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data_Covid19"))
            {
                Directory.CreateDirectory("Data_Covid19");
            }
            var path = @"Data_Covid19\Corona.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists Corona " +
                    "(Country varchar(50), Cases int, " +
                    "TodayCases int, " +
                    "Deaths int, TodayDeaths int, Recovered int, Active int, " +
                    "Critical int, CasesPerOneMillion int)";

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
                    string sql = string.Format("insert into Countries (Name, Capital, Region, Subregion, Population, Gini, Flag)" +
                        " values (@Name, @Capital, @Region, @Subregion, @Population, @Gini, @Flag)");

                    command = new SQLiteCommand(sql, connection);

                    command.Parameters.Add(new SQLiteParameter("@Name", country.Name));
                    command.Parameters.Add(new SQLiteParameter("@Capital", country.Capital));
                    command.Parameters.Add(new SQLiteParameter("@Region", country.Region));
                    command.Parameters.Add(new SQLiteParameter("@Subregion", country.Subregion));
                    command.Parameters.Add(new SQLiteParameter("@Population", country.Population));
                    command.Parameters.Add(new SQLiteParameter("@Gini", country.Gini));
                    command.Parameters.Add(new SQLiteParameter("@Flag", country.Flag));

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Erro", ex.Message);
            }
        }
        public void SaveDataCovid19(List<Covid19Data> Corona)
        {
            try
            {
                foreach (var covid19 in Corona)
                {
                    string sql = string.Format("insert into Corona (Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
                    "Critical, CasesPerOneMillion)" +
                        " values (@Country, @Cases, @TodayCases, @Deaths, @TodayDeaths, @Recovered, @Active, @Critical, @CasesPerOneMillion)");

                    command = new SQLiteCommand(sql, connection);

                    command.Parameters.Add(new SQLiteParameter("@Country", covid19.Country));
                    command.Parameters.Add(new SQLiteParameter("@Cases", covid19.Cases));
                    command.Parameters.Add(new SQLiteParameter("@TodayCases", covid19.TodayCases));
                    command.Parameters.Add(new SQLiteParameter("@Deaths", covid19.Deaths));
                    command.Parameters.Add(new SQLiteParameter("@TodayDeaths", covid19.TodayDeaths));
                    command.Parameters.Add(new SQLiteParameter("@Recovered", covid19.Recovered));
                    command.Parameters.Add(new SQLiteParameter("@Active", covid19.Active));
                    command.Parameters.Add(new SQLiteParameter("@Critical", covid19.Critical));
                    command.Parameters.Add(new SQLiteParameter("@CasesPerOneMillion", covid19.CasesPerOneMillion));

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
        public List<Covid19Data> GetDataCovid19()
        {
            List<Covid19Data> covid19 = new List<Covid19Data>();

            try
            {
                string sql = "select Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
                    "Critical, CasesPerOneMillion from Corona";

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())//enquanto tiver registos pra ler
                {
                    covid19.Add(new Covid19Data
                    {
                        Country = (string)reader["Country"],
                        Cases = (int)reader["Cases"],
                        TodayCases = (int)reader["TodayCases"],
                        Deaths = (int)reader["Deaths"],
                        TodayDeaths = (int)reader["TodayDeaths"],
                        Recovered = (int)reader["Recovered"],
                        Active = (int)reader["Active"],
                        Critical = (int)reader["Critical"],
                        CasesPerOneMillion = (int)reader["CasesPerOneMillion"]
                    });
                }

                connection.Close();

                return covid19;
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
        public void DeleteDataCovid19()
        {
            try
            {
                string sql = "delete from Corona";

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
