using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private TourOperatorDBEntities _dbContext;
        private int _clientId;

        public OrdersPage()
        {
            InitializeComponent();
            _dbContext = new TourOperatorDBEntities();

            if (SessionManager.ClientId != null)
            {
                _clientId = SessionManager.ClientId.Value;
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Ошибка: пользователь не авторизован.");
            }
        }


        private void LoadOrders()
        {
            var orders = _dbContext.Orders
                .Where(o => o.ClientId == _clientId)
                .Select(o => new
                {
                    o.OrderId,
                    o.Tours.TourName,
                    o.OrderDate,
                    o.Status.StatusName
                })
                .AsEnumerable() // Выполняем запрос к базе данных
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    TourName = o.TourName,
                    OrderDate = o.OrderDate.ToShortDateString(), // Преобразование даты в строку
                    Status = o.StatusName
                })
                .ToList();

            OrdersDataGrid.ItemsSource = orders;
        }

        // Плоская модель данных для DataGrid
        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public string TourName { get; set; }
            public string OrderDate { get; set; }
            public string Status { get; set; }
        }
        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                MessageBox.Show("Ошибка: не удалось определить кнопку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (button.Tag == null || !int.TryParse(button.Tag.ToString(), out int orderId))
            {
                MessageBox.Show("Ошибка: не удалось получить идентификатор заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Удаление заказа из базы данных
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                MessageBox.Show("Ошибка: заказ не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

            MessageBox.Show("Заказ успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadOrders(); // Обновляем список заказов
        }

    }
}
