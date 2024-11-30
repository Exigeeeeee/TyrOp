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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private readonly TourOperatorDBEntities _context;
        private Orders _order;
       

        public AddOrder(Orders order = null)
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
            _order = order;

            // Загружаем данные для ComboBox
            LoadComboBoxData();

            if (_order != null)
            {
                // Если передан заказ для редактирования, заполняем поля
                ClientComboBox.SelectedValue = _order.ClientId;
                TourComboBox.SelectedValue = _order.TourId;
                StatusComboBox.SelectedValue = _order.StatusID;
                OrderDatePicker.SelectedDate = _order.OrderDate;
            }
        }

        /// <summary>
        /// Загружает данные в ComboBox.
        /// </summary>
        private void LoadComboBoxData()
        {
            ClientComboBox.ItemsSource = _context.Clients.ToList();
            TourComboBox.ItemsSource = _context.Tours.ToList();
            StatusComboBox.ItemsSource = _context.Status.ToList();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Save".
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedItem == null || TourComboBox.SelectedItem == null || StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_order == null)
                {
                    // Создаем новый заказ
                    _order = new Orders
                    {
                        ClientId = (int)ClientComboBox.SelectedValue,
                        TourId = (int)TourComboBox.SelectedValue,
                        StatusID = (int)StatusComboBox.SelectedValue,
                        OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now
                    };

                    _context.Orders.Add(_order); // Добавляем в контекст
                }
                else
                {
                    // Если заказ редактируется
                    var existingOrder = _context.Orders
                        .FirstOrDefault(o => o.OrderId == _order.OrderId);

                    if (existingOrder != null)
                    {
                        existingOrder.ClientId = (int)ClientComboBox.SelectedValue;
                        existingOrder.TourId = (int)TourComboBox.SelectedValue;
                        existingOrder.StatusID = (int)StatusComboBox.SelectedValue;
                        existingOrder.OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now;

                        // Обновляем состояние объекта
                        _context.Entry(existingOrder).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                // Сохраняем изменения
                _context.SaveChanges();
                MessageBox.Show("Заказ успешно сохранен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true; // Устанавливаем успешный результат
                this.Close(); // Закрываем окно
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
