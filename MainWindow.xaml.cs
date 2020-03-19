namespace Paises
{
    using Models;
    using System.Windows;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Services;
    using System.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private RootObject Countries;
        private NetworkService NetworkService;
        private APIService APIService;
        private DialogService DialogService;
        private DataService DataServices;
        DataTable dt = new DataTable();
        DataRow dr;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Countries = new RootObject();
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

           

            dt.Columns.Add("Name");
            dt.Columns.Add("Capital");
            dt.Columns.Add("Region");
            dt.Columns.Add("Subregion");
            dt.Columns.Add("Population");
            dt.Columns.Add("Gini");
            dt.Columns.Add("Flag");
            dr = dt.NewRow();
            dr["Name"] = Countries.Name;
            dr["Capital"] = Countries.Capital;
            dr["Region"] = Countries.Region;
            dr["Subregion"] = Countries.Subregion;
            dr["Population"] = Countries.Population;
            dr["Gini"] = Countries.Gini;
            dr["Flag"] = Countries.Flag;
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            listView.ItemsSource = dt.DefaultView;


            //if (Countries.Equals(string.Empty)) //lista de taxas nao foi carregada
            //{
            //    lblResult.Content = "No internet connection" +
            //        Environment.NewLine + "and the countries were not previously loaded." +
            //        Environment.NewLine + "Try it later.";

            //    lblResult.Content = "First boot should have internet connection.";

            //    return;
            //}

            //cbOrigem.DataSource = Rates;
            //cbOrigem.DisplayMember = "Name"; // Isto *** em vez do override

            ////corrige bug da microsoft
            //cbDestino.BindingContext = new BindingContext();

            //cbDestino.DataSource = Rates;
            //cbDestino.DisplayMember = "Name";

            pb.Value = 100;

            lblResult.Content = "Countries update!";

            if (load) // API carregou
            {
                lblStatus.Content = string.Format($"Countries downloaded from the internet in" +
                    $" {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = string.Format("Countries downloaded from Data Base.");
            }

            pb.Value = 100;
        }

        private async Task LoadApiCountries()
        {
            pb.Value = 0;

            /*onde está o endereço base da API
            vai buscar o controlador
            enquanto carrega as taxas a aplicação tem que estar a correr á mesma*/
            var response = await APIService.GetCountries("http://restcountries.eu/rest/v2/all", "/api/countries");

            Countries = (RootObject)response.Result; // vai buscar a referencia da lista

            DataServices.DeleteData();

           // DataServices.SaveData(Countries);
        }

        private void LoadLocalCountries()
        {
          //  Countries = DataServices.GetData();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
