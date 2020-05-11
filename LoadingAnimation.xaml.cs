namespace Paises
{
    using Countries;
    using System;
    using System.Windows;
    using System.Windows.Threading;


    /// <summary>
    /// Interaction logic for LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingAnimation : Window
    {
        DispatcherTimer timer = new DispatcherTimer();


        public LoadingAnimation()
        {
            InitializeComponent();

            Animation();
            Loading();
        }
        private void Animation()
        {
            gifEarth.Source = new Uri(Environment.CurrentDirectory + @"\planet.gif");


        }
        private void MediaElement_MediaEnded(object sender, EventArgs e)
        {
            gifEarth.Position = new TimeSpan(0, 0, 1);
            gifEarth.Play();
        }
        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            Close();

        }
        private void Loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 8);
            timer.Start();
        }
    }
}
