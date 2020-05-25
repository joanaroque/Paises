namespace Countries.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public static class DataService
    {
        private const string Path = @"Data\Countries.sqlite";

        private const string PathCovid19 = @"Data\Covid19.sqlite";

        private const string DataName = "Data";

        /// <summary>
        /// create table countries
        /// </summary>
        public static void CreateDataCountries()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }
            try
            {
                connection.Open();

                string sqlcommand = "create table if not exists Countries " +
                    "(Alpha3Code varchar(3), Name varchar(50), Capital varchar(50), " +
                    "Region varchar(50), " +
                    "Subregion varchar(50), Population int," +
                    "Gini real, Flag varchar(200))";

                SQLiteCommand command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// create table currencies
        /// </summary>
        public static void CreateDataCurrencies()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection.Open();

                string sqlcommand = "create table if not exists currencies " +
                    "(Alpha3Code varchar(3), Code varchar(10), CurrencyName varchar(50)," +
                    "Symbol varchar(10))";

                SQLiteCommand command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        ///  create table translations
        /// </summary>
        public static void CreateDataTranslations()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection.Open();

                string sqlcommand = "create table if not exists translations " +
                    "(Alpha3Code varchar(3), De varchar(50), Es varchar(50)," +
                    "Fr varchar(50), Ja varchar(50), It varchar(50)," +
                    "Br varchar(50), Pt varchar(50), Nl varchar(50)," +
                    "Hr varchar(50), Fa varchar(50))";

                SQLiteCommand command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// create table languages
        /// </summary>
        public static void CreateDataLanguages()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);


            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection.Open();

                string sqlcommand = "create table if not exists Languages " +
                    "(Alpha3Code varchar(3), LanguageName varchar(50)," +
                    "NativeName varchar(50))";

                SQLiteCommand command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// create table covid19 statistics
        /// </summary>
        public static void CreateDataCovid19()
        {

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + PathCovid19);

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection.Open();

                string sqlcommand = "create table if not exists Covid19 " +
                    "(Country varchar(50), Cases int, " +
                    "TodayCases int, " +
                    "Deaths int, TodayDeaths int, Recovered int, Active int, " +
                    "Critical int, CasesPerOneMillion int)";

                SQLiteCommand command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Save the data fetch from the api in the database
        /// </summary>
        /// <param name="Countries">list of countries</param>
        public static void SaveDataCountries(List<Country> Countries)
        {
          
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            string sqlCountry = string.Format("insert into Countries (Alpha3Code, Name, Capital, Region, Subregion, Population, Gini, Flag)" +
                   " values (@Alpha3Code, @Name, @Capital, @Region, @Subregion, @Population, @Gini, @Flag)");

            try
            {
                SQLiteCommand command = new SQLiteCommand(sqlCountry, connection);

                connection.Open();

                foreach (var country in Countries)
                {
                    command.Parameters.Add(new SQLiteParameter("@Alpha3Code", country.Alpha3Code));
                    command.Parameters.Add(new SQLiteParameter("@Name", country.Name));
                    command.Parameters.Add(new SQLiteParameter("@Capital", country.Capital));
                    command.Parameters.Add(new SQLiteParameter("@Region", country.Region));
                    command.Parameters.Add(new SQLiteParameter("@Subregion", country.Subregion));
                    command.Parameters.Add(new SQLiteParameter("@Population", country.Population));
                    command.Parameters.Add(new SQLiteParameter("@Gini", country.Gini));
                    command.Parameters.Add(new SQLiteParameter("@Flag", country.Flag));

                    command.ExecuteNonQuery();

                   

                    LogService.Log($"Country: {country.Alpha3Code}");

                    SaveCurrencies(connection, country.Alpha3Code, country.Currencies);
                   
                    SaveLanguages(connection, country.Alpha3Code, country.Languages);
                   
                    SaveTranslations(connection, country.Alpha3Code, country.Translations);
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Save the data fetch from the api in the database
        /// </summary>
        public static void SaveCurrencies(SQLiteConnection connection, string alpha3Code, List<Currency> currencies)
        {
            string sqlCurrencies = string.Format("insert into currencies (Alpha3Code, Code, CurrencyName, Symbol)" +
                   " values (@Alpha3Code, @Code, @CurrencyName, @Symbol)");

            SQLiteCommand command = new SQLiteCommand(sqlCurrencies, connection);

            foreach (var currency in currencies)
            {

                command.Parameters.Add(new SQLiteParameter("@Alpha3Code", alpha3Code));
                command.Parameters.Add(new SQLiteParameter("@Code", currency.Code));
                command.Parameters.Add(new SQLiteParameter("@CurrencyName", currency.Name));
                command.Parameters.Add(new SQLiteParameter("@Symbol", currency.Symbol));

                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Save the data fetch from the api in the database
        /// </summary>
        public static void SaveLanguages(SQLiteConnection connection, string alpha3Code, List<Language> languages)
        {
            string sqlLanguages = string.Format("insert into languages (Alpha3Code, LanguageName, NativeName)" +
                        " values (@Alpha3Code, @LanguageName, @NativeName)");

            SQLiteCommand command = new SQLiteCommand(sqlLanguages, connection);


            foreach (var language in languages)
            {

                command.Parameters.Add(new SQLiteParameter("@Alpha3Code", alpha3Code));
                command.Parameters.Add(new SQLiteParameter("@LanguageName", language.Name));
                command.Parameters.Add(new SQLiteParameter("@NativeName", language.NativeName));

                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Save the data fetch from the api in the database
        /// </summary>
        public static void SaveTranslations(SQLiteConnection connection, string alpha3Code, Translations translations)
        {
            string sqlTranslation = string.Format("insert into translations (Alpha3Code, De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa)" +
                        " values (@Alpha3Code, @De, @Es, @Fr, @Ja, @It, @Br, @Pt, @Nl, @Hr, @Fa)");

            SQLiteCommand command = new SQLiteCommand(sqlTranslation, connection);

            command.Parameters.Add(new SQLiteParameter("@Alpha3Code", alpha3Code));
            command.Parameters.Add(new SQLiteParameter("@De", translations.De));
            command.Parameters.Add(new SQLiteParameter("@Es", translations.Es));
            command.Parameters.Add(new SQLiteParameter("@Fr", translations.Fr));
            command.Parameters.Add(new SQLiteParameter("@Ja", translations.Ja));
            command.Parameters.Add(new SQLiteParameter("@It", translations.It));
            command.Parameters.Add(new SQLiteParameter("@Br", translations.Br));
            command.Parameters.Add(new SQLiteParameter("@Pt", translations.Pt));
            command.Parameters.Add(new SQLiteParameter("@Nl", translations.Nl));
            command.Parameters.Add(new SQLiteParameter("@Hr", translations.Hr));
            command.Parameters.Add(new SQLiteParameter("@Fa", translations.Fa));

            command.ExecuteNonQuery();
        }
        /// <summary>
        /// Save the data fetch from the api in the database
        /// </summary>
        /// <param name="Corona">List of statistics </param>
        public static void SaveDataCovid19(List<Covid19Data> Corona)
        {

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + PathCovid19);

            try
            {
                string sql = string.Format("insert into Covid19 (Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
                    "Critical, CasesPerOneMillion)" +
                     " values (@Country, @Cases, @TodayCases, @Deaths, @TodayDeaths, @Recovered, @Active, @Critical, @CasesPerOneMillion)");

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();

                foreach (var covid19 in Corona)
                {

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
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }

        }
        /// <summary>
        /// get all data from DB
        /// </summary>
        /// <param name="progress"></param>
        /// <returns>country data list</returns>
        public static async Task<List<Country>> GetDataCountries(IProgress<double> progress)
        {
            List<Country> countries = new List<Country>();
            List<Currency> currencies = new List<Currency>();
            List<Language> languages = new List<Language>();

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            string countriesSql = "select Alpha3Code, Name, Capital, Region, Subregion, Population, Gini, Flag from Countries";

            try
            {
                connection.Open();

                SQLiteDataReader countriesReader = new SQLiteCommand(countriesSql, connection).ExecuteReader();

                while (countriesReader.Read())
                {
                    Country currentCountry = new Country
                    {
                        Alpha3Code = GetDBStringValue(countriesReader, 0),
                        Name = GetDBStringValue(countriesReader, 1),
                        Capital = GetDBStringValue(countriesReader, 2),
                        Region = GetDBStringValue(countriesReader, 3),
                        Subregion = GetDBStringValue(countriesReader, 4),
                        Population = countriesReader.GetInt32(5),
                        Gini = countriesReader.GetDouble(6),
                        Flag = countriesReader.GetString(7),
                    };

                    string sqlCurr = "select Code, CurrencyName, Symbol from currencies where alpha3code = @country";

                    try
                    {
                        SQLiteCommand allCountriesCommand = new SQLiteCommand(sqlCurr, connection);
                        allCountriesCommand.Parameters.AddWithValue("@country", (string)countriesReader["alpha3code"]);

                        SQLiteDataReader readerCurr = allCountriesCommand.ExecuteReader();


                        while (readerCurr.Read())
                        {
                            currencies.Add(new Currency
                            {
                                Code = GetDBStringValue(readerCurr, 0),
                                Name = GetDBStringValue(readerCurr, 1),
                                Symbol = GetDBStringValue(readerCurr, 2)
                            });
                        }
                        currentCountry.Currencies = currencies;

                    }
                    catch (Exception e)
                    {
                        DialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }

                    string sqlLang = "select LanguageName, NativeName from languages where alpha3code = @country";

                    SQLiteCommand selectBycountryCommand = new SQLiteCommand(sqlLang, connection);
                    selectBycountryCommand.Parameters.AddWithValue("@country", countriesReader.GetString(0));

                    try
                    {
                        SQLiteDataReader readerLang = selectBycountryCommand.ExecuteReader();

                        while (readerLang.Read())
                        {
                            languages.Add(new Language
                            {
                                Name = GetDBStringValue(readerLang, 0),
                                NativeName = GetDBStringValue(readerLang, 1),
                            });
                        }
                        currentCountry.Languages = languages;
                    }
                    catch (Exception e)
                    {
                        DialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }

                    string sqlTran = "select De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa from translations where alpha3code = @country";

                    SQLiteCommand selectTransCommand = new SQLiteCommand(sqlTran, connection);
                    selectTransCommand.Parameters.AddWithValue("@country", countriesReader.GetString(0));

                    try
                    {
                        SQLiteDataReader readerTrans = selectTransCommand.ExecuteReader();

                        while (readerTrans.Read())
                        {
                            Translations translations = new Translations
                            {
                                De = GetDBStringValue(readerTrans, 0),
                                Es = GetDBStringValue(readerTrans, 1),
                                Fr = GetDBStringValue(readerTrans, 2),
                                Ja = GetDBStringValue(readerTrans, 3),
                                It = GetDBStringValue(readerTrans, 4),
                                Br = GetDBStringValue(readerTrans, 5),
                                Pt = GetDBStringValue(readerTrans, 6),
                                Nl = GetDBStringValue(readerTrans, 7),
                                Hr = GetDBStringValue(readerTrans, 8),
                                Fa = GetDBStringValue(readerTrans, 9),
                            };
                            currentCountry.Translations = translations;
                        }
                    }
                    catch (Exception e)
                    {
                        DialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }
                    countries.Add(currentCountry);

                    progress.Report((countries.Count * 100) / 250);
                }

                LogService.Log("***All countries GET IT!!!!!!!!");

                return countries;

            }
            catch (Exception e)
            {
                DialogService.ShowMessage(e.Message, "Error");
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// If database as null values, show null, if not, gets what it is
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="id"></param>
        /// <returns>null if database as null, or something else</returns>
        private static string GetDBStringValue(SQLiteDataReader reader, int id)
        {
            return reader.IsDBNull(id) ? null : reader.GetString(id);
        }
        /// <summary>
        /// obtain country virus statistics
        /// </summary>
        /// <returns>country virus data list</returns>
        public static List<Covid19Data> GetDataCovid19()
        {
            List<Covid19Data> covid19 = new List<Covid19Data>();

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + PathCovid19);
            try
            {
                connection.Open();

                string sql = "select Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
                    "Critical, CasesPerOneMillion from Covid19";

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    covid19.Add(new Covid19Data
                    {
                        Country = reader.GetString(0),
                        Cases = reader.GetInt32(1),
                        TodayCases = reader.GetInt32(2),
                        Deaths = reader.GetInt32(3),
                        TodayDeaths = reader.GetInt32(4),
                        Recovered = reader.GetInt32(5),
                        Active = reader.GetInt32(6),
                        Critical = reader.GetInt32(7),
                        CasesPerOneMillion = reader.GetInt32(8)
                    });
                }

                return covid19;
            }

            catch (Exception e)
            {
                DialogService.ShowMessage(e.Message, "Error");
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// erases data from the table
        /// </summary>
        public static void DeleteDataCountries()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            try
            {
                connection.Open();

                string sql = "delete from Countries";

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                DialogService.ShowMessage(e.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// erases data from the table
        /// </summary>
        public static void DeleteDataCovid19()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + PathCovid19);

            try
            {
                connection.Open();

                string sql = "delete from Covid19";

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                DialogService.ShowMessage(e.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
