namespace Countries.Services
{
    using System.Windows;

    internal static class DialogService
    {
        internal static void ShowMessage(string title, string message) // todo: tirar esta treta
        {
            MessageBox.Show(title, message);
        }
    }
}