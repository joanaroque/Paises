namespace Countries
{
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
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
        private List<RootObject> Countries;
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

            if (Corona.Count == 0) //lista de dados de covid19 nao foi carregada
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

        private async Task LoadApiCountriesCovid19()
        {
            var response = await apiService.GetCovid19Data("https://coronavirus-19-api.herokuapp.com/", "countries");

            Corona = (List<Covid19Data>)response.Result; // vai buscar a referencia da lista

            dataServices.DeleteDataCovid19();

            dataServices.SaveDataCovid19(Corona);
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
                load = true;
            }

            if (Countries.Count == 0) //lista de paises nao foi carregada
            {
                InfoToUser();

                return;
            }

            cbCountry.ItemsSource = Countries;
            cbCountry.DisplayMemberPath = "Name";


            pb.Value = 100;

            lblResult.Content = "Countries update!";

            if (load) // API carregou
            {
                lblStatus.Content = string.Format($"Countries downloaded from the internet in {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = "Countries downloaded from Data Base.";
            }

            pb.Value = 100;
        }

        private async Task LoadApiCountries()
        {
            pb.Value = 0;

            /*onde está o endereço base da API
            vai buscar a apiPath
            enquanto carrega a api, a aplicação tem que estar a correr á mesma*/
            var response = await apiService.GetCountries("http://restcountries.eu/rest/v2/", "all");

            Countries = (List<RootObject>)response.Result; // vai buscar a referencia da lista

            dataServices.DeleteDataCountries();

            dataServices.SaveDataCountries(Countries);
        }

        private void LoadLocalCountries()
        {
            Countries = dataServices.GetDataCountries();
        }

        private void CbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RootObject currentCountry = Countries[cbCountry.SelectedIndex];

            try
            {
                Flag.Source = new BitmapImage(new Uri($@"\Resources\{currentCountry.Name.ToLower()}.png", UriKind.Relative));
            }
            catch (Exception)
            {
                Flag.Source = new BitmapImage(new Uri(@"\Resources\image.jpg", UriKind.Relative));
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
            lblFr.Content = $"French: {currentCountry.Translations.Fr}";
            lblJa.Content = $"Japanese: {currentCountry.Translations.Ja}";
            lblIt.Content = $"Italian: {currentCountry.Translations.It}";
            lblBr.Content = $"Brazilian: {currentCountry.Translations.Br}";
            lblPt.Content = $"Portuguese: {currentCountry.Translations.Pt}";
            lblNl.Content = $"Dutch: {currentCountry.Translations.Nl}";
            lblHr.Content = $"Croatian: {currentCountry.Translations.Hr}";
            lblFa.Content = $"Arabian: {currentCountry.Translations.Fa}";//Todo se alguma propriedade nao existir, mostrar X 


            Covid19Data currentCountryCovid = Corona[cbCountry.SelectedIndex];

            lblCases.Content = $"Cases: {currentCountryCovid.Cases}";
            lblTodayCases.Content = $"Today Cases: {currentCountryCovid.TodayCases}";
            lblDeaths.Content = $"Deaths: {currentCountryCovid.Deaths}";
            lblTodayDeaths.Content = $"Today Deaths: {currentCountryCovid.TodayDeaths}";
            lblRecovered.Content = $"Recovered: {currentCountryCovid.Recovered}";
            lblActive.Content = $"Active: {currentCountryCovid.Active}";
            lblCritical.Content = $"Critical: {currentCountryCovid.Critical}";
            lblCasesPerOneMillion.Content = $"Cases Per One Million: {currentCountryCovid.CasesPerOneMillion}";

        }
    }
}
