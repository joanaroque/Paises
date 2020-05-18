namespace Countries
{
    using Models;
    using Services;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using WpfAnimatedGif;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private List<Country> Countries;
        private List<Covid19Data> Corona;
        private IProgress<double> progress;
        private bool eSaving = false;
        #endregion


        public MainWindow()
        {
            progress = new Progress<double>(ReportProgress);

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            LoadingGif();

            LoadCountries();

            LoadCovid19Data();

            pb.Value = 100;
            
        }

        private void ReportProgress(double number)
        {
            if (number != 0)
            {
               
                pb.Value = pb.Value + number;
                LogService.Log($"Receiving number {number}");
            }
            else
            {
                LogService.Log($"Receiving 0");
            }
        }

        private void LoadCovid19Data()
        {
            DataService.CreateDataCovid19();

            var connection = NetworkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCovid19Data();

                if (Corona.Count == 0)
                {
                    InfoToUser();

                    return;
                }
            }
            else
            {
                Task task = Task.Run(() => LoadApiCountriesCovid19());

            }
        }

        private void InfoToUser()
        {
            lblResult.Content = "No internet connection" +
                   Environment.NewLine + "and the countries were not previously loaded." +
                   Environment.NewLine + "Try it later.";

            lblStatus.Content = "First boot should have internet connection.";
        }
        /// <summary>
        /// If the result of response diferent from null, gest the list with info covid
        /// else, gets an empty list
        /// </summary>
        /// <returns>populating a list</returns>
        private async Task LoadApiCountriesCovid19()
        {
            await Task.Delay(5000);

            var response = await APIService.GetCovid19Data(APIService.urlExtra, APIService.pathExtra);

            Corona = response.Result != null ? (List<Covid19Data>)response.Result : new List<Covid19Data>();

            if (Corona.Count > 0)
            {
                DataService.DeleteDataCovid19();
                DataService.SaveDataCovid19(Corona);
            }
            await Task.Delay(5000);

        }

        private void LoadLocalCovid19Data()
        {
            Corona = DataService.GetDataCovid19();
        }

        private async void LoadCountries()
        {
            DataService.CreateDataCountries();
            DataService.CreateDataCurrencies();
            DataService.CreateDataLanguages();
            DataService.CreateDataTranslations();

            bool load;

            var connection = NetworkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                eSaving = true;
                LoadLocalCountries();
                load = false;
            }

            else
            {
                await LoadApiCountries();
                await DownloadFlags();
                load = true;
            }

            if (Countries.Count == 0)
            {
                InfoToUser();
                return;
            }

            cbCountry.ItemsSource = Countries;
            cbCountry.DisplayMemberPath = "Name";

            lblResult.Content = "Countries updated!";

            if (load)
            {
                lblStatus.Content = string.Format($"Countries downloaded from the internet in {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = "Countries downloaded from Database.";
            }    
        }
        private void LoadingGif()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\planet.gif", UriKind.Absolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(gif, image);
        }

        private async Task LoadApiCountries()
        {
            Task<Response> apiResponse = Task.Run(() => APIService.GetCountries(APIService.url, APIService.path));
            int count = 0;

            while (!apiResponse.IsCompleted)
            {
                count++;

                progress.Report(1);

                Thread.Sleep(1000);
            }

            Response response = await apiResponse;

            Countries = response.Result != null ? (List<Country>)response.Result : new List<Country>();


            if (Countries.Count > 0)
            {
                Task task = Task.Run(() =>

                {
                    LogService.Log("********Deleting previous countries");

                    DataService.DeleteDataCountries();

                    LogService.Log("*** Saving countries now...");

                    DataService.SaveDataCountries(Countries);
                    eSaving = true;

                    LogService.Log("*** Saved countries.");
                });
            }
        }     

        private async void LoadLocalCountries()
        {
            Countries = await DataService.GetDataCountries(progress);
        }

        private async Task DownloadFlags()
        {
            await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    foreach (var country in Countries)
                    {
                        try
                        {
                            if (!File.Exists($@"FlagsJpeg\{country.Alpha3Code}.jpg"))
                            {
                                client.DownloadFile(country.Flag, $@"LocalFlags\{country.Alpha3Code}.svg");

                                ConvertSvgToJpg(country.Alpha3Code);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                }
            });
        }

        private void ConvertSvgToJpg(string Alpha3Code)
        {
            try
            {
                string flagSvg = $@"LocalFlags\{Alpha3Code}.svg";

                var svg = SvgDocument.Open(flagSvg);

                Bitmap map = svg.Draw(400, 230);

                string flagJpg = $@"FlagsJpeg\{Alpha3Code}.jpg";

                map.Save(flagJpg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void SetFlagImage(Country currentCountry)
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"FlagsJpeg\{currentCountry.Alpha3Code}.jpg", UriKind.Absolute);
            bitmap.EndInit();

            Flag.Source = bitmap;
        }

        private void CbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            gif.Visibility = Visibility.Hidden;

            Country currentCountry = Countries[cbCountry.SelectedIndex];
            List<Covid19Data> corona = Corona != null ? Corona : new List<Covid19Data>();

            try
            {
                SetFlagImage(currentCountry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        
            lblCapital.Content = $"Capital: {currentCountry.Capital}";
            lblRegion.Content = $"Region: {currentCountry.Region}";
            lblSubregion.Content = $"Subregion: {currentCountry.Subregion}";
            lblPopulation.Content = $"Population: {currentCountry.Population}";
            lblGini.Content = $"Gini: {currentCountry.Gini}";

            foreach (var currencies in currentCountry.Currencies)
            {
                lblCurrencyCode.Content = $"Currency Code: {currencies.Code}";
                lblCurrencyName.Content = $"Currency Name: {currencies.Name}";
                lblSymbol.Content = $"Currency Symbol: {currencies.Symbol}";
            }

            foreach (var languages in currentCountry.Languages)
            {
                lblNameLanguage.Content = $"Laguage Name: {languages.Name}";
                lblNative.Content = $"Native Name: {languages.NativeName}";
            }

            lblDe.Content = $"German: {currentCountry.Translations.De}";
            lblEs.Content = $"Spanish: {currentCountry.Translations.Es}";
            lblFr.Content = $"French:  {currentCountry.Translations.Fr}";
            lblJa.Content = $"Japanese:{currentCountry.Translations.Ja}";
            lblIt.Content = $"Italian: {currentCountry.Translations.It}";
            lblBr.Content = $"Brazilian: {currentCountry.Translations.Br}";
            lblPt.Content = $"Portuguese: {currentCountry.Translations.Pt}";
            lblNl.Content = $"Dutch: {currentCountry.Translations.Nl}";
            lblHr.Content = $"Croatian: {currentCountry.Translations.Hr}";
            lblFa.Content = $"Arabian: {currentCountry.Translations.Fa}";

            if (Corona != null)
            {
                foreach (Covid19Data covidCountry in corona)
                {
                    lblCases.Content = "";
                    lblTodayCases.Content = "";
                    lblDeaths.Content = "";
                    lblTodayDeaths.Content = "";
                    lblRecovered.Content = "";
                    lblActive.Content = "";
                    lblCritical.Content = "";
                    lblCasesPerOneMillion.Content = "";

                    if (covidCountry.Country.Equals(currentCountry.Name))
                    {
                        lblCases.Content = $"Cases: {covidCountry.Cases}";
                        lblTodayCases.Content = $"Today Cases: {covidCountry.TodayCases}";
                        lblDeaths.Content = $"Deaths: {covidCountry.Deaths}";
                        lblTodayDeaths.Content = $"Today Deaths: {covidCountry.TodayDeaths}";
                        lblRecovered.Content = $"Recovered: {covidCountry.Recovered}";
                        lblActive.Content = $"Active: {covidCountry.Active}";
                        lblCritical.Content = $"Critical: {covidCountry.Critical}";
                        lblCasesPerOneMillion.Content = $"Cases Per One Million: {covidCountry.CasesPerOneMillion}";
                        break;
                    }

                    if (covidCountry.Country.Equals(currentCountry.Alpha3Code))
                    {
                        lblCases.Content = $"Cases: {covidCountry.Cases}";
                        lblTodayCases.Content = $"Today Cases: {covidCountry.TodayCases}";
                        lblDeaths.Content = $"Deaths: {covidCountry.Deaths}";
                        lblTodayDeaths.Content = $"Today Deaths: {covidCountry.TodayDeaths}";
                        lblRecovered.Content = $"Recovered: {covidCountry.Recovered}";
                        lblActive.Content = $"Active: {covidCountry.Active}";
                        lblCritical.Content = $"Critical: {covidCountry.Critical}";
                        lblCasesPerOneMillion.Content = $"Cases Per One Million: {covidCountry.CasesPerOneMillion}";
                        break;
                    }
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            LogService.Log("*** window closing.");

            if (!eSaving)
            {
                DialogService.ShowMessage("Database is being updated. Please try again later.", "Warning");
            }
            else
            {
                Close();
            }
        }
    }
}
