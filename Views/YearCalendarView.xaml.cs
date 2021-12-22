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
        private DateTime currentYear;

        public YearCalendarView()
        {
            InitializeComponent();
            currentYear = new DateTime(DateTime.Now.Year, 1, 1);
            UpdateYearNumber();
            UpdateMonthCalendars();
        }

        private void UpdateYearNumber()
        {
            YearNumber.Text = currentYear.Year.ToString();
        }

        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            currentYear = currentYear.AddYears(-1);
            UpdateYearNumber();
            UpdateMonthCalendars();
        }

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            currentYear = currentYear.AddYears(1);
            UpdateYearNumber();
            UpdateMonthCalendars();
        }

        private void UpdateMonthCalendar(Calendar monthCalendar, int month)
        {
            var displayYear = currentYear.AddMonths(month - 1);
            monthCalendar.DisplayDate = displayYear;
            monthCalendar.SelectedDate = displayYear;
            monthCalendar.DisplayDateStart = monthCalendar.DisplayDate;
            monthCalendar.DisplayDateEnd = monthCalendar.DisplayDate.AddMonths(1).AddDays(-1);
        }

        private void UpdateMonthCalendars()
        {
            UpdateMonthCalendar(Month1, 1);
            UpdateMonthCalendar(Month2, 2);
            UpdateMonthCalendar(Month3, 3);
            UpdateMonthCalendar(Month4, 4);
            UpdateMonthCalendar(Month5, 5);
            UpdateMonthCalendar(Month6, 6);
            UpdateMonthCalendar(Month7, 7);
            UpdateMonthCalendar(Month8, 8);
            UpdateMonthCalendar(Month9, 9);
            UpdateMonthCalendar(Month10, 10);
            UpdateMonthCalendar(Month11, 11);
            UpdateMonthCalendar(Month12, 12);
        }
    }
}