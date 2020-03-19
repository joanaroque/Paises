namespace Paises
{
    using Models;
    using System.Windows;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private List<RootObject> Country;
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

            if (Country.Count == 0) //lista de taxas nao foi carregada
            {
                lblResult.Content = "No internet connection" +
                    Environment.NewLine + "and the countries were not previously loaded." +
                    Environment.NewLine + "Try it later.";

                lblResult.Content = "First boot should have internet connection.";

                return;
            }

            cbOrigem.DataSource = Rates;
            cbOrigem.DisplayMember = "Name"; // Isto *** em vez do override

            //corrige bug da microsoft
            cbDestino.BindingContext = new BindingContext();

            cbDestino.DataSource = Rates;
            cbDestino.DisplayMember = "Name";

            pb.Value = 100;

            lblResult.Content = "Countries update!";

            if (load) // API carregou
            {
                lblStatus.Content = string.Format($"Taxas carregadas da internet a " +
                    $" {DateTime.Now}");
            }
            else
            {
                lblStatus.Content = string.Format("Taxas carregadas da Base de Dados.");
            }

            pb.Value = 100;
        }

        private Task LoadApiCountries()
        {
            throw new NotImplementedException();
        }

        private void LoadLocalCountries()
        {
            throw new NotImplementedException();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
