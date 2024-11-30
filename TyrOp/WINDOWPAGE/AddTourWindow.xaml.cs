using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TyrOp.Model;

namespace TyrOp.WINDOWPAGE
{
    /// <summary>
    /// Логика взаимодействия для AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        private readonly TourOperatorDBEntities _context;

        public AddTourWindow()
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newTour = new Tours
            {
                TourName = TourNameBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                StartDate = StartDatePicker.SelectedDate ?? DateTime.Now,
                EndDate = EndDatePicker.SelectedDate ?? DateTime.Now.AddDays(7),
                Status = "Active"
            };

            _context.Tours.Add(newTour);
            _context.SaveChanges();

            MessageBox.Show("Тур успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }
    }
}