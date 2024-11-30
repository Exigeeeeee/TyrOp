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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TyrOp.Model;

namespace TyrOp.WINDOWPAGE
{
    /// <summary>
    /// Логика взаимодействия для ToursPage.xaml
    /// </summary>
    public partial class ToursPage : Page
    {
        private TourOperatorDBEntities _dbContext;
        private int _clientId;

        public ToursPage()
        {
            InitializeComponent();
            _dbContext = new TourOperatorDBEntities();

            if (SessionManager.ClientId != null)
            {
                _clientId = SessionManager.ClientId.Value;
                LoadTours();
            }
            else
            {
                MessageBox.Show("Ошибка: пользователь не авторизован.");
            }
        }

        private void LoadTours()
        {
            var tours = _dbContext.Tours.ToList();
            ToursDataGrid.ItemsSource = tours;
        }

        private void OrderTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (ToursDataGrid.SelectedItem is Tours selectedTour)
            {
                var order = new Orders
                {
                    ClientId = _clientId,
                    TourId = selectedTour.TourId,
                    OrderDate = DateTime.Now,
                    StatusID = 1 // "В обработке"
                };

                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                MessageBox.Show($"Тур \"{selectedTour.TourName}\" успешно заказан.");
            }
            else
            {
                MessageBox.Show("Выберите тур для заказа.");
            }
        }
    }
}
