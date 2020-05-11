namespace Countries
{
    using Models;
    using Paises.Models;
    using Services;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private List<Country> Countries;
        private List<Covid19Data> Corona;
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            LoadCountries();
            LoadCovid19Data();
        }

        private async void LoadCovid19Data()
        {
            DataService.CreateDataCovid19();

            var connection = NetworkService.CheckConnection();

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
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            var response = await APIService.GetCovid19Data(APIService.urlExtra, APIService.pathExtra);


            Corona = response.Result != null ? (List<Covid19Data>)response.Result : new List<Covid19Data>();

            if (Corona.Count > 0)
            {
                DataService.DeleteDataCovid19();
                DataService.SaveDataCovid19(Corona, progress);
            }
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

            lblResult.Content = "Countries update!";



            if (load)
            {
                lblStatus.Content = string.Format($"Countries downloaded from the internet in {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = "Countries downloaded from Data Base.";
            }
            //gif.Visibility = Visibility.Hidden;
        }

        private async Task LoadApiCountries()
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;



            var response = await APIService.GetCountries(APIService.url, APIService.path);


            Countries = response.Result != null ? (List<Country>)response.Result : new List<Country>();



            if (Countries.Count > 0)
            {
                Task task = Task.Run(() =>

                {
                    Console.WriteLine($"Deleting previous countries now: {DateTime.Now}");
                    DataService.DeleteDataCountries();
                    Console.WriteLine($"Saving countries now: {DateTime.Now}");
                    DataService.SaveDataCountries(Countries, progress);
                    Console.WriteLine($"Saved countries now: {DateTime.Now}");

                });
            }


        }



        private void ReportProgress(object sender, ProgressReport e)
        {
            pb.Value = e.Percentage;

            if (Countries.Count == 250 && pb.Value == 0)
            {
                lblStatus.Content = "Updating database...";
            }
        }

        private async void LoadLocalCountries()
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            await DataService.GetDataCountries(progress);


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
