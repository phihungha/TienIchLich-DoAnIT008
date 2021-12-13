using Microsoft.Win32;
using System.Windows;

namespace TienIchLich.Services
{
    /// <summary>
    /// Provides dialog-related functions.
    /// </summary>
    public class DialogService
    {
        /// <summary>
        /// Show an error message.
        /// </summary>
        /// <param name="text">Dialog text</param>
        public void ShowError(string text)
        {
            MessageBox.Show(text, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Show a confirmation dialog.
        /// </summary>
        /// <param name="text">Dialog text</param>
        /// <returns>True if the user clicks Ok</returns>
        public bool ShowConfirmation(string text)
        {
            MessageBoxResult result = MessageBox.Show(text, "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }

        /// <summary>
        /// Show an open file dialog.
        /// </summary>
        /// <param name="fileFormats">File formats to use</param>
        /// <returns></returns>
        public string ShowOpenFile(string fileFormats)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = fileFormats;

            bool? result = dialog.ShowDialog();
            if (result == true)
                return dialog.FileName;
            return null;
        }
    }
}