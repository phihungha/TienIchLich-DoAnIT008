using System;
using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.Views
{
    /// <summary>
    /// Interaction logic for YearCalendarView.xaml
    /// </summary>
    public partial class YearCalendarView : UserControl
    {
        public YearCalendarView()
        {
            InitializeComponent();
            Month1.DisplayDate = new DateTime(DateTime.Now.Year, 1, 1);
            YearNumber.Text = Month1.DisplayDate.Year.ToString();
        }

        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            Month1.DisplayDate = Month1.DisplayDate.AddYears(-1);
            YearNumber.Text = Month1.DisplayDate.Year.ToString();
        }

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            Month1.DisplayDate = Month1.DisplayDate.AddYears(1);
            YearNumber.Text = Month1.DisplayDate.Year.ToString();
        }
    }
}