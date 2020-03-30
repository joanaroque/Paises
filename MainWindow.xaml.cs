namespace Countries
{
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;

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
            dataServices.DeleteDataCurrencies();
            dataServices.DeleteDataLanguages();
            dataServices.DeleteDataTranslations();

            dataServices.SaveDataCountries(Countries);
           // dataServices.SaveDataCurrencies(Countries);
        }

        private void LoadLocalCountries()
        {
            Countries = dataServices.GetData();
        }

        private void CbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            lblCapital.Content = $"Capital: {Countries[cbCountry.SelectedIndex].Capital}";
            lblRegion.Content = $"Region: {Countries[cbCountry.SelectedIndex].Region}";
            lblSubregion.Content = $"Subregion: {Countries[cbCountry.SelectedIndex].Subregion}";
            lblPopulation.Content = $"Population: {Countries[cbCountry.SelectedIndex].Population}";
            lblGini.Content = $"Gini: {Countries[cbCountry.SelectedIndex].Gini}";

            lblCurrencyCode.Content = $"Currency Code: {Countries[cbCountry.SelectedIndex].Currencies[0].Code}";
            lblCurrencyName.Content = $"Currency Name: {Countries[cbCountry.SelectedIndex].Currencies[0].Name}";
            lblSymbol.Content = $"Currency Symbol: {Countries[cbCountry.SelectedIndex].Currencies[0].Symbol}";

            lblNameLanguage.Content = $"Laguage Name: {Countries[cbCountry.SelectedIndex].Languages[0].Name}";
            lblNative.Content = $"Native Name: {Countries[cbCountry.SelectedIndex].Languages[0].NativeName}";

            lblDe.Content = $"German: {Countries[cbCountry.SelectedIndex].Translations.De}";
            lblEs.Content = $"Spanish: {Countries[cbCountry.SelectedIndex].Translations.Es}";
            lblFr.Content = $"French: {Countries[cbCountry.SelectedIndex].Translations.Fr}";
            lblJa.Content = $"Japanese: {Countries[cbCountry.SelectedIndex].Translations.Ja}";
            lblIt.Content = $"Italian: {Countries[cbCountry.SelectedIndex].Translations.It}";
            lblBr.Content = $"Brazilian: {Countries[cbCountry.SelectedIndex].Translations.Br}";
            lblPt.Content = $"Portuguese: {Countries[cbCountry.SelectedIndex].Translations.Pt}";
            lblNl.Content = $"Dutch: {Countries[cbCountry.SelectedIndex].Translations.Nl}";
            lblHr.Content = $"Croatian: {Countries[cbCountry.SelectedIndex].Translations.Hr}";
            lblFa.Content = $"Arabian: {Countries[cbCountry.SelectedIndex].Translations.Fa}";//Todo se alguma propriedade nao existir, mostrar X 

            lblCases.Content = $"Cases: {Corona[cbCountry.SelectedIndex].Cases}";
            lblTodayCases.Content = $"Today Cases: {Corona[cbCountry.SelectedIndex].TodayCases}";
            lblDeaths.Content = $"Deaths: {Corona[cbCountry.SelectedIndex].Deaths}";
            lblTodayDeaths.Content = $"Today Deaths: {Corona[cbCountry.SelectedIndex].TodayDeaths}";
            lblRecovered.Content = $"Recovered: {Corona[cbCountry.SelectedIndex].Recovered}";
            lblActive.Content = $"Active: {Corona[cbCountry.SelectedIndex].Active}";
            lblCritical.Content = $"Critical: {Corona[cbCountry.SelectedIndex].Critical}";
            lblCasesPerOneMillion.Content = $"Cases Per One Million: {Corona[cbCountry.SelectedIndex].CasesPerOneMillion}";

            //string s = cbCountry.SelectedItem.ToString();
            //Flags.Source = new BitmapCache(s);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbCountry.Text = "-- SELECT COUNTRY --";

        }
    }
}
