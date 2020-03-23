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
            var response = await APIService.GetCountries("https://coronavirus-19-api.herokuapp.com/", "countries");

            Corona = (List<Covid19Data>)response.Result; // vai buscar a referencia da lista

            DataServices.DeleteData();

           // DataServices.SaveData(Corona);

        }   //TODO dependendo do pais escolhido, aparecem os dados desse mesmo pais, para escolher outro, volta para tras..

        private void LoadLocalCountries()
        {
          //  Corona = DataServices.GetData();
        }

        private void PopulateLabels() // TODO fazer toda este novo modelo!!!
        {
            foreach (var country in Corona) 
            {
                lblName.Content = country.Country;
                lblCases.Content = country.Cases;
                lblTodayCases.Content = country.TodayCases;
                lblDeaths.Content = country.Deaths;
                lblTodayDeaths.Content = country.TodayDeaths;
                lblRecovered.Content = country.Recovered;
                lblActive.Content = country.Active;
                lblCritical.Content = country.Critical;
                lblCasesPerOneMillion.Content = country.CasesPerOneMillion;
            }
        }
    }
}
