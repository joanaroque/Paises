namespace Countries
{
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Covid19.xaml
    /// </summary>
    public partial class Covid19 : Window
    {
        #region Attributes
        private List<Covid19Data> Corona;
        private NetworkService NetworkService;
        private APIService APIService;
        private DialogService DialogService;
        private DataService DataServices;

        #endregion

        public Covid19()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NetworkService = new NetworkService();
            APIService = new APIService();
            DialogService = new DialogService();
            DataServices = new DataService();

            LoadCovid19Data();
        }

        private async void LoadCovid19Data()
        {
            bool load;
            var connection = NetworkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCovid19Data();
                load = false;
            }

            else
            {
                await LoadApiCountriesCovid19();
                load = true;
            }

            if (Corona.Count == 0) //lista de dados de covid19 nao foi carregada
            {
                lblResult.Content = "No internet connection" +
                    Environment.NewLine + "and the covid19 data were not previously loaded." +
                    Environment.NewLine + "Try it later.";

                lblStatus.Content = "First boot should have internet connection.";

                return;
            }
            PopulateLabels();
        }

        private void btnCountries_Click(object sender, RoutedEventArgs e)
        {
            MainWindow first = new MainWindow();
            first.Show();
            this.Close();
        }
       
        private async Task LoadApiCountriesCovid19()
        {
            var response = await APIService.GetCovid19Data("https://coronavirus-19-api.herokuapp.com/", "countries");

            Corona = (List<Covid19Data>)response.Result; // vai buscar a referencia da lista

            DataServices.DeleteDataCovid19();

           DataServices.SaveDataCovid19(Corona);

        }   //TODO dependendo do pais escolhido, aparecem os dados desse mesmo pais, para escolher outro, volta para tras..

        private void LoadLocalCovid19Data()
        {
           Corona = DataServices.GetDataCovid19();
        }

        private void PopulateLabels()
        {
            foreach (var covid19 in Corona)
            {
                lblName.Content = covid19.Country;
                lblCases.Content = covid19.Cases;
                lblTodayCases.Content = covid19.TodayCases;
                lblDeaths.Content = covid19.Deaths;
                lblTodayDeaths.Content = covid19.TodayDeaths;
                lblRecovered.Content = covid19.Recovered;
                lblActive.Content = covid19.Active;
                lblCritical.Content = covid19.Critical;
                lblCasesPerOneMillion.Content = covid19.CasesPerOneMillion;
            }
        }
    }
}
