namespace Countries.Services
{
    using System.Windows;

    internal class DialogService
    {
        internal void ShowMessage(string title, string message)
        {
            MessageBox.Show(title, message);
        }
    }
}