namespace Countries.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Threading.Tasks;

    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;

        private const string Path = @"Data\Countries.sqlite";

        private const string PathCovid19 = @"Data\Covid19.sqlite";

        private const string DataName = "Data";

        public void CreateDataCountries()
        {
            dialogService = new DialogService();

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }
            try
            {
                connection = new SQLiteConnection("Data Source=" + Path);
                connection.Open();

                string sqlcommand = "create table if not exists Countries " +
                    "(Alpha2Code varchar(2), Name varchar(50), Capital varchar(50), " +
                    "Region varchar(50), " +
                    "Subregion varchar(50), Population int," +
                    "Gini real, Flag varchar(200))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void CreateDataCurrencies()
        {
            dialogService = new DialogService();

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection = new SQLiteConnection("Data Source=" + Path);
                connection.Open();

                string sqlcommand = "create table if not exists currencies " +
                    "(Alpha2Code varchar(2), Code varchar(10), CurrencyName varchar(50)," +
                    "Symbol varchar(10))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void CreateDataTranslations()
        {
            dialogService = new DialogService();

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection = new SQLiteConnection("Data Source=" + Path);
                connection.Open();

                string sqlcommand = "create table if not exists translations " +
                    "(Alpha2Code varchar(2), De varchar(50), Es varchar(50)," +
                    "Fr varchar(50), Ja varchar(50), It varchar(50)," +
                    "Br varchar(50), Pt varchar(50), Nl varchar(50)," +
                    "Hr varchar(50), Fa varchar(50))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void CreateDataLanguages()
        {
            dialogService = new DialogService();

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection = new SQLiteConnection("Data Source=" + Path);
                connection.Open();

                string sqlcommand = "create table if not exists Languages " +
                    "(Alpha2Code varchar(2), LanguageName varchar(50)," +
                    "NativeName varchar(50))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void CreateDataCovid19()
        {
            dialogService = new DialogService();

            if (!Directory.Exists(DataName))
            {
                Directory.CreateDirectory(DataName);
            }

            try
            {
                connection = new SQLiteConnection("Data Source=" + PathCovid19);
                connection.Open();

                string sqlcommand = "create table if not exists Covid19 " +
                    "(Country varchar(50), Cases int, " +
                    "TodayCases int, " +
                    "Deaths int, TodayDeaths int, Recovered int, Active int, " +
                    "Critical int, CasesPerOneMillion int)";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SaveDataCountries(List<Country> Countries)
        {
            connection = new SQLiteConnection("Data Source=" + Path);

            string sqlCountry = string.Format("insert into Countries (Alpha2Code, Name, Capital, Region, Subregion, Population, Gini, Flag)" +
                   " values (@Alpha2Code, @Name, @Capital, @Region, @Subregion, @Population, @Gini, @Flag)");
            try
            {
                command = new SQLiteCommand(sqlCountry, connection);

                connection.Open();

                foreach (var country in Countries)
                {
                    command.Parameters.Add(new SQLiteParameter("@Alpha2Code", country.Alpha2Code));
                    command.Parameters.Add(new SQLiteParameter("@Name", country.Name));
                    command.Parameters.Add(new SQLiteParameter("@Capital", country.Capital));
                    command.Parameters.Add(new SQLiteParameter("@Region", country.Region));
                    command.Parameters.Add(new SQLiteParameter("@Subregion", country.Subregion));
                    command.Parameters.Add(new SQLiteParameter("@Population", country.Population));
                    command.Parameters.Add(new SQLiteParameter("@Gini", country.Gini));
                    command.Parameters.Add(new SQLiteParameter("@Flag", country.Flag));

                     command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SaveCurrencies()
        {
            List<Country> Countries = new List<Country>();

            connection = new SQLiteConnection("Data Source=" + Path);

            string sqlCurrencies = string.Format("insert into currencies (Alpha2Code, Code, CurrencyName, Symbol)" +
                   " values (@Alpha2Code, @Code, @CurrencyName, @Symbol)");

            try
            {
                command = new SQLiteCommand(sqlCurrencies, connection);

                connection.Open();

                foreach (var country in Countries)
                {
                    foreach (var currency in country.Currencies)
                    {
                        command = new SQLiteCommand(sqlCurrencies, connection);

                        command.Parameters.Add(new SQLiteParameter("@Alpha2Code", country.Alpha2Code));
                        command.Parameters.Add(new SQLiteParameter("@Code", currency.Code));
                        command.Parameters.Add(new SQLiteParameter("@CurrencyName", currency.Name));
                        command.Parameters.Add(new SQLiteParameter("@Symbol", currency.Symbol));

                         command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SaveLanguages()
        {
            List<Country> Countries = new List<Country>();

            connection = new SQLiteConnection("Data Source=" + Path);

            string sqlLanguages = string.Format("insert into languages (Alpha2Code, LanguageName, NativeName)" +
                        " values (@Alpha2Code, @LanguageName, @NativeName)");

            try
            {
                command = new SQLiteCommand(sqlLanguages, connection);

                connection.Open();

                foreach (var country in Countries)
                {
                    foreach (var language in country.Languages)
                    {

                        command = new SQLiteCommand(sqlLanguages, connection);

                        command.Parameters.Add(new SQLiteParameter("@Alpha2Code", country.Alpha2Code));
                        command.Parameters.Add(new SQLiteParameter("@LanguageName", language.Name));
                        command.Parameters.Add(new SQLiteParameter("@NativeName", language.NativeName));

                         command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SaveTranslations()
        {
            List<Country> Countries = new List<Country>();

            connection = new SQLiteConnection("Data Source=" + Path);

            string sqlTranslation = string.Format("insert into translations (Alpha2Code, De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa)" +
                        " values (@Alpha2Code, @De, @Es, @Fr, @Ja, @It, @Br, @Pt, @Nl, @Hr, @Fa)");

            try
            {
                command = new SQLiteCommand(sqlTranslation, connection);

                connection.Open();

                foreach (var country in Countries)
                {
                    command = new SQLiteCommand(sqlTranslation, connection);

                    command.Parameters.Add(new SQLiteParameter("@Alpha2Code", country.Alpha2Code));
                    command.Parameters.Add(new SQLiteParameter("@De", country.Translations.De));
                    command.Parameters.Add(new SQLiteParameter("@Es", country.Translations.Es));
                    command.Parameters.Add(new SQLiteParameter("@Fr", country.Translations.Fr));
                    command.Parameters.Add(new SQLiteParameter("@Ja", country.Translations.Ja));
                    command.Parameters.Add(new SQLiteParameter("@It", country.Translations.It));
                    command.Parameters.Add(new SQLiteParameter("@Br", country.Translations.Br));
                    command.Parameters.Add(new SQLiteParameter("@Pt", country.Translations.Pt));
                    command.Parameters.Add(new SQLiteParameter("@Nl", country.Translations.Nl));
                    command.Parameters.Add(new SQLiteParameter("@Hr", country.Translations.Hr));
                    command.Parameters.Add(new SQLiteParameter("@Fa", country.Translations.Fa));

                     command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SaveDataCovid19(List<Covid19Data> Corona)
        {
            connection = new SQLiteConnection("Data Source=" + PathCovid19);

            try
            {
                connection.Open();

                foreach (var covid19 in Corona)
                {
                    string sql = string.Format("insert into Covid19 (Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
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
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<List<Country>> GetDataCountries()
        {
            List<Country> countries = new List<Country>();

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Path);

            string countriesSql = "select Alpha2Code, Name, Capital, Region, Subregion, Population, Gini, Flag from Countries";

            try
            {
                connection.Open();

                SQLiteDataReader countriesReader = new SQLiteCommand(countriesSql, connection).ExecuteReader();

                while (countriesReader.Read())
                {
                    Country currentCountry = new Country
                    {
                        Alpha2Code = GetDBStringValue(countriesReader, 0),
                        Name = GetDBStringValue(countriesReader, 1),
                        Capital = GetDBStringValue(countriesReader, 2),
                        Region = GetDBStringValue(countriesReader, 3),
                        Subregion = GetDBStringValue(countriesReader, 4),
                        Population = countriesReader.GetInt32(5),
                        Gini = countriesReader.GetDouble(6),
                        Flag = countriesReader.GetString(7),
                    };

                    string sqlCurr = "select Code, CurrencyName, Symbol from currencies where alpha2code = @country";

                    try
                    {
                        command = new SQLiteCommand(sqlCurr, connection);
                        command.Parameters.AddWithValue("@country", (string)countriesReader["alpha2code"]);

                        SQLiteDataReader readerCurr = command.ExecuteReader();
                        List<Currency> currencies = new List<Currency>();

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
                        dialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }

                    string sqlLang = "select LanguageName, NativeName from languages where alpha2code = @country";

                    command = new SQLiteCommand(sqlLang, connection);
                    command.Parameters.AddWithValue("@country", countriesReader.GetString(0));

                    try
                    {
                        SQLiteDataReader readerLang = command.ExecuteReader();

                        List<Language> languages = new List<Language>();

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
                        dialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }

                    string sqlTran = "select De, Es, Fr, Ja, It, Br, Pt, Nl, Hr, Fa from translations where alpha2code = @country";

                    command = new SQLiteCommand(sqlTran, connection);
                    command.Parameters.AddWithValue("@country", countriesReader.GetString(0));

                    try
                    {
                        SQLiteDataReader readerTrans = command.ExecuteReader();

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
                        dialogService.ShowMessage(e.Message, "Error");
                        return null;
                    }
                    countries.Add(currentCountry);
                }

                return countries;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage(e.Message, "Error");
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// If database as null values, show null, if not, show what it is
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="id"></param>
        /// <returns>null if database as null, or something else</returns>
        private string GetDBStringValue(SQLiteDataReader reader, int id)
        {
            return reader.IsDBNull(id) ? null : reader.GetString(id);
        }

        public List<Covid19Data> GetDataCovid19()
        {
            List<Covid19Data> covid19 = new List<Covid19Data>();

            connection = new SQLiteConnection("Data Source=" + PathCovid19);
            try
            {
                connection.Open();

                string sql = "select Country, Cases, TodayCases, Deaths, TodayDeaths, Recovered, Active, " +
                    "Critical, CasesPerOneMillion from Covid19";

                command = new SQLiteCommand(sql, connection);

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
                dialogService.ShowMessage(e.Message, "Error");
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        public void DeleteDataCountries()
        {
            connection = new SQLiteConnection("Data Source=" + Path);

            try
            {
                connection.Open();

                string sql = "delete from Countries";

                command = new SQLiteCommand(sql, connection);

                 command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage(e.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
        public void DeleteDataCovid19()
        {
            connection = new SQLiteConnection("Data Source=" + PathCovid19);

            try
            {
                connection.Open();

                string sql = "delete from Covid19";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage(e.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
