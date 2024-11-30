using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TyrOp.Model; // Подключение вашей модели данных
using System.Data.Entity;

namespace TyrOp.WINDOWPAGE
{
    public partial class OrdersPageAdmin : Page
    {
        private TourOperatorDBEntities _dbContext;

        public OrdersPageAdmin()
        {
            _dbContext = new TourOperatorDBEntities();
            InitializeComponent();

            LoadOrders(); // Загружаем все заказы сразу при инициализации страницы
        }

        private void LoadOrders()
        {
            try
            {
                // Загружаем все заказы с информацией о клиенте и туре
                var orders = _dbContext.Orders
                    .Include(o => o.Clients)  // Загружаем информацию о клиенте
                    .Include(o => o.Tours)    // Загружаем информацию о туре
                    .Include(o => o.Status)  // Загружаем информацию о статусе
                    .ToList();

                var orderViewModels = orders.Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    ClientName = o.Clients.FullName,  // Имя клиента
                    TourName = o.Tours.TourName,      // Название тура
                    OrderDate = o.OrderDate.ToShortDateString(), // Преобразование даты в строку
                    Status = o.Status.StatusName     // Статус заказа
                }).ToList();

                // Привязываем данные к DataGrid
                OrdersDataGrid.ItemsSource = orderViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Плоская модель данных для DataGrid
        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public string ClientName { get; set; } // Имя клиента
            public string TourName { get; set; }
            public string OrderDate { get; set; }
            public string Status { get; set; }
        }

        // Открывает окно для добавления нового заказа
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new AddOrder(); // Создаём окно для добавления
            if (orderWindow.ShowDialog() == true)
            {
                LoadOrders(); // Перезагружаем заказы после добавления
            }
        }

        // Открывает окно для редактирования выбранного заказа
        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (OrderViewModel)OrdersDataGrid.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем сам объект заказа из базы данных
            var orderToEdit = _dbContext.Orders
                .Include(o => o.Clients)
                .Include(o => o.Tours)
                .Include(o => o.Status)
                .FirstOrDefault(o => o.OrderId == selectedOrder.OrderId);

            if (orderToEdit == null)
            {
                MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Открываем окно для редактирования с объектом заказа
            var orderWindow = new AddOrder(orderToEdit);
            if (orderWindow.ShowDialog() == true)
            {
                LoadOrders(); // Перезагружаем заказы после редактирования
            }
        }


        // Удаляет выбранный заказ
        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (OrderViewModel)OrdersDataGrid.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтверждаем удаление
            if (MessageBox.Show($"Вы уверены, что хотите удалить заказ №{selectedOrder.OrderId}?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var orderToDelete = _dbContext.Orders
                        .FirstOrDefault(o => o.OrderId == selectedOrder.OrderId);

                    if (orderToDelete != null)
                    {
                        _dbContext.Orders.Remove(orderToDelete);
                        _dbContext.SaveChanges();
                        LoadOrders(); // Перезагружаем заказы после удаления
                        MessageBox.Show("Заказ успешно удалён.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
