using System.Windows;
using System.Windows.Controls;
using TienIchLich.ViewModels;

namespace TienIchLich.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EventEditorView : UserControl
    {
        public EventEditorView()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.StartTimePicker.FormatString = "dddd, dd MMMM yyyy";
            this.EndTimePicker.FormatString = "dddd, dd MMMM yyyy";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.StartTimePicker.FormatString = "dddd, dd MMMM yyyy HH:mm";
            this.EndTimePicker.FormatString = "dddd, dd MMMM yyyy HH:mm";
        }
    }
}
