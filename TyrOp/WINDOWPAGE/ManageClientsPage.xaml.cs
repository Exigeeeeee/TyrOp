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
    /// Логика взаимодействия для ManageClientsPage.xaml
    /// </summary>
    public partial class ManageClientsPage : Page
    {
        private readonly TourOperatorDBEntities _context;

        public ManageClientsPage()
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
            LoadClients();
        }

        /// <summary>
        /// Загружает список клиентов в DataGrid.
        /// </summary>
        private void LoadClients()
        {
            try
            {
                ClientsDataGrid.ItemsSource = _context.Clients.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавляет нового клиента.
        /// </summary>
        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            var clientWindow = new AddClientWindow(); // Используем конструктор без параметров
            if (clientWindow.ShowDialog() == true)
            {
                LoadClients(); // Обновляем список клиентов
            }
        }

        /// <summary>
        /// Редактирует выбранного клиента.
        /// </summary>
        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = (Clients)ClientsDataGrid.SelectedItem;
            if (selectedClient == null)
            {
                MessageBox.Show("Выберите клиента для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var clientWindow = new AddClientWindow(selectedClient); // Передаем выбранного клиента
            if (clientWindow.ShowDialog() == true)
            {
                LoadClients(); // Обновляем список клиентов после редактирования
            }
        }

        /// <summary>
        /// Удаляет выбранного клиента.
        /// </summary>
        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = (Clients)ClientsDataGrid.SelectedItem;
            if (selectedClient == null)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {selectedClient.FullName}?",
                                         "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Clients.Remove(selectedClient);
                    _context.SaveChanges();
                    MessageBox.Show("Клиент успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
