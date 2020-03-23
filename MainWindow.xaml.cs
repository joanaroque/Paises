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
        private NetworkService NetworkService;
        private APIService APIService;
        private DialogService DialogService;
        private DataService DataServices;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NetworkService = new NetworkService();
            APIService = new APIService();
            DialogService = new DialogService();
            DataServices = new DataService();

            LoadCountries();
        }

        private async void LoadCountries()
        {
            bool load;

            lblResult.Content = "Update Countries...";

            var connection = NetworkService.CheckConnection();

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
                lblResult.Content = "No internet connection" +
                    Environment.NewLine + "and the countries were not previously loaded." +
                    Environment.NewLine + "Try it later.";

                lblStatus.Content = "First boot should have internet connection.";

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
            enquanto carrega as taxas a aplicação tem que estar a correr á mesma*/
            var response = await APIService.GetCountries("http://restcountries.eu/rest/v2/", "all");

            Countries = (List<RootObject>)response.Result; // vai buscar a referencia da lista

            DataServices.DeleteData();

            DataServices.SaveData(Countries);
        }

        private void LoadLocalCountries()
        {
            Countries = DataServices.GetData();
        }

        private void CbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (var country in Countries)
            {
                lblName.Content = country.Name;
                lblCapital.Content = country.Capital;
                lblRegion.Content = country.Region;
                lblSubregion.Content = country.Subregion;
                lblPopulation.Content = country.Population;
                lblGini.Content = country.Gini;
                lblFlag.Content = country.Flag; // TODO se nao houver bandeira, mostrar uma imagem de erro
            }
        }
        private void btnCovid_Click(object sender, RoutedEventArgs e)
        {
            Covid19 corona = new Covid19();
            corona.Show();
            this.Close();
        }
    }
}
