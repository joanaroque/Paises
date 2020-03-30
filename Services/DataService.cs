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

        public void CreateDataCountries() // passa aqui
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

                string sqlcommand = "create table if not exists Countries " +
                    "(Name varchar(50), Capital varchar(50), " +
                    "Region varchar(50), " +
                    "Subregion varchar(50), Population int," +
                    "Gini real, Flag varchar(200))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error", ex.Message);
            }
        }
        public void CreateDataCurrencies() // passa aqui
        {
            dialogService = new DialogService();

            if (!Directory.Exists("DataCurrencies"))
            {
                Directory.CreateDirectory("DataCurrencies");
            }
            var path = @"DataCurrencies\Currencies.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists currencies " +
                    "(Code varchar(10), CurrencyName varchar(50)," +
                    "Symbol varchar(10))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error", ex.Message);
            }
        }
        public void CreateDataTranslations() // passa aqui
        {
            dialogService = new DialogService();

            if (!Directory.Exists("DataTranslations"))
            {
                Directory.CreateDirectory("DataTranslations");
            }
            var path = @"DataTranslations\Translations.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists translations " +
                    "(De varchar(50), Es varchar(50)," +
                    "Fr varchar(50), Ja varchar(50), It varchar(50)," +
                    "Br varchar(50), Pt varchar(50), Nl varchar(50)," +
                    "Hr varchar(50), Fa varchar(50))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error", ex.Message);
            }
        }
        public void CreateDataLanguages() // passa aqui
        {
            dialogService = new DialogService();

            if (!Directory.Exists("DataLanguages"))
            {
                Directory.CreateDirectory("DataLanguages");
            }
            var path = @"DataLanguages\Languages.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists Languages " +
                    "(LanguageName varchar(50)," +
                    "NativeName varchar(50))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error", ex.Message);
            }
        }
        public void CreateDataCovid19() // passa aqui
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
                dialogService.ShowMessage("Error", ex.Message);
            }
        }
        public void SaveDataCountries(List<RootObject> Countries)
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
                   
                    command.ExecuteNonQuery(); // nao passa aqui
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("merda 1", ex.Message);
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
                dialogService.ShowMessage("merda 2", ex.Message); // passa este
            }
        }
        public void SaveDataCurrencies(List<Currency> Currencies)
        {
            try
            {
                foreach (var currency in Currencies)
                {
                    string sql = string.Format("insert into currencies (Code, CurrencyName, Symbol)" +
                        " values (@Code, @CurrencyName, @Symbol)");

                    command = new SQLiteCommand(sql, connection);

                    command.Parameters.Add(new SQLiteParameter("@Code", currency.Code));
                    command.Parameters.Add(new SQLiteParameter("@CurrencyName", currency.Name));
                    command.Parameters.Add(new SQLiteParameter("@Symbol", currency.Symbol));

                    command.ExecuteNonQuery(); // nao passa aqui
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("merda 3", ex.Message);
            }
        }
        public void SaveDataLanguages(List<Language> Languages)
        {
            try
            {
                foreach (var language in Languages)
                {
                    string sql = string.Format("insert into languages (LanguageName, NativeName)" +
                        " values (@LanguageName, @NativeName)");

                    command = new SQLiteCommand(sql, connection);

                    command.Parameters.Add(new SQLiteParameter("@LanguageName", language.Name));
                    command.Parameters.Add(new SQLiteParameter("@NativeName", language.NativeName));

                    command.ExecuteNonQuery(); // nao passa aqui
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("merda 4", ex.Message);
            }
        }
        public void SaveDataTranslations(List<Translations> translations)
        {
            try
            {
                foreach (var translation in translations)
                {
                    string sql = string.Format("insert into translations (De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa)" +
                        " values (@De, @Es, @Fr, @Ja, @It, @Br, @Pt, @Nl, @Hr, @Fa)");

                    command = new SQLiteCommand(sql, connection);

                    command.Parameters.Add(new SQLiteParameter("@De", translation.De));
                    command.Parameters.Add(new SQLiteParameter("@Es", translation.Es));
                    command.Parameters.Add(new SQLiteParameter("@Fr", translation.Fr));
                    command.Parameters.Add(new SQLiteParameter("@Ja", translation.Ja));
                    command.Parameters.Add(new SQLiteParameter("@It", translation.It));
                    command.Parameters.Add(new SQLiteParameter("@Br", translation.Br));
                    command.Parameters.Add(new SQLiteParameter("@Pt", translation.Pt));
                    command.Parameters.Add(new SQLiteParameter("@Nl", translation.Nl));
                    command.Parameters.Add(new SQLiteParameter("@Hr", translation.Hr));
                    command.Parameters.Add(new SQLiteParameter("@Fa", translation.Fa));

                    command.ExecuteNonQuery(); // nao passa aqui
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("merda 5", ex.Message);
            }
        }

        public List<RootObject> GetData()
        {
            List<RootObject> countries = new List<RootObject>();
            List<Language> languages = new List<Language>();
            List<Currency> currencies = new List<Currency>();
            List<Translations> translations = new List<Translations>();
            try
            {
                string sql = "select Name, Capital, Region, Subregion, Population, Gini, Flag, LanguageName," +
                    "NativeName , Code, CurrencyName," +
                    "Symbol, De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa from Countries";

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

                    languages.Add(new Language
                    {
                        Name = (string)reader["LanguageName"],
                        NativeName = (string)reader["NativeName"]
                    });

                    currencies.Add(new Currency
                    {
                        Code = (string)reader["Code"],
                        Name = (string)reader["CurrencyName"],
                        Symbol = (string)reader["Symbol"]
                    });

                    translations.Add(new Translations
                    {
                        De = (string)reader["De"],
                        Es = (string)reader["Es"],
                        Fr = (string)reader["Fr"],
                        Ja = (string)reader["Ja"],
                        It = (string)reader["It"],
                        Br = (string)reader["Br"],
                        Pt = (string)reader["Pt"],
                        Nl = (string)reader["Nl"],
                        Hr = (string)reader["Hr"],
                        Fa = (string)reader["Fa"]
                    });
                }

                connection.Close();

                return countries;
            }

            catch (Exception e)
            {
                dialogService.ShowMessage("deve ser aqui que ta a dar merda", e.Message);
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
                dialogService.ShowMessage("Error", e.Message);
                return null;
            }
        }
        public void DeleteDataCountries()
        {
            try
            {
                string sql = "delete from Countries";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("sera que da merda aqui??", e.Message);
            }
        }
        public void DeleteDataCurrencies()
        {
            try
            {
                string sql = "delete from currecies";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
        public void DeleteDataLanguages()
        {
            try
            {
                string sql = "delete from languages";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
        public void DeleteDataTranslations()
        {
            try
            {
                string sql = "delete from translations";

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
