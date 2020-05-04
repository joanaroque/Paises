namespace Countries
{
    using Models;
    using Services;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private List<Country> Countries;
        private List<Covid19Data> Corona;
        private NetworkService networkService;
        private APIService apiService;
        private DataService dataServices;
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            networkService = new NetworkService();
            apiService = new APIService();
            dataServices = new DataService();

            LoadCountries();
            LoadCovid19Data();
        }

        private async void LoadCovid19Data()
        {
            dataServices.CreateDataCovid19();

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCovid19Data();
            }

            else
            {
                await LoadApiCountriesCovid19();
            }

            if (Corona.Count == 0)
            {
                InfoToUser();

                return;
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
            Task<Response> taskResponse = apiService.GetCovid19Data(APIService.urlExtra, APIService.pathExtra);

            var response = await taskResponse;

            Corona = response.Result != null ? (List<Covid19Data>)response.Result : new List<Covid19Data>(); 

            if (Corona.Count > 0)
            {
                dataServices.DeleteDataCovid19();
                dataServices.SaveDataCovid19(Corona);
            }
        }

        private void LoadLocalCovid19Data()
        {
            Corona = dataServices.GetDataCovid19();
        }

        private async void LoadCountries()
        {
            dataServices.CreateDataCountries();
            dataServices.CreateDataCurrencies();
            dataServices.CreateDataLanguages();
            dataServices.CreateDataTranslations();

            bool load;

            lblResult.Content = "Update Countries...";

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCountries();
                load = false;
            }

            else
            {
                await LoadApiCountries();
                DownloadFlags();
                load = true;
            }

            if (Countries.Count == 0)
            {
                InfoToUser();
                return;
            }

            cbCountry.ItemsSource = Countries;
            cbCountry.DisplayMemberPath = "Name";

            lblResult.Content = "Countries update!";

            pb.Value = 100;

            if (load)
            {
                lblStatus.Content = string.Format($"Countries downloaded from the internet in {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = "Countries downloaded from Data Base.";
            }
        }

        private async Task LoadApiCountries()
        {
            Task<Response> taskResponse = apiService.GetCountries(APIService.url, APIService.path);

            while (!taskResponse.IsCompleted)
            {
                pb.Value++;
                await Task.Delay(1);
            }

            pb.Value = 50;

            var response = await taskResponse;

            Countries = response.Result != null ? (List<Country>)response.Result : new List<Country>();

            if (Countries.Count > 0)
            {
                dataServices.DeleteDataCountries();

                dataServices.SaveDataCountries(Countries);
                dataServices.SaveCurrencies();
                dataServices.SaveLanguages();
                dataServices.SaveTranslations();
            }

            pb.Value = 75;
        }

        private async void LoadLocalCountries()
        {
            Countries = await dataServices.GetDataCountries();

            while (!dataServices.GetDataCountries().IsCompleted)
            {
                pb.Value++;
                await Task.Delay(1);
            }
        }

        private void DownloadFlags()
        {
            WebClient client = new WebClient();

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
            client.Dispose();
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
            Country currentCountry = Countries[cbCountry.SelectedIndex];

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

            foreach (Covid19Data covidCountry in Corona)
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
            }
        }
    }
}
