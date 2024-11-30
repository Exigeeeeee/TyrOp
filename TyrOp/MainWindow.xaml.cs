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
using TyrOp.WINDOWPAGE;

namespace TyrOp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка пользователя в базе данных
            var context = TourOperatorDBEntities.GetContext();

            // Проверка в таблице Users
            var user = context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                MessageBox.Show("Добро пожаловать, " + user.UserName + "!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenUserWindow(user.RoleId);
                return;
            }

            // Проверка в таблице Clients
            var client = context.Clients.FirstOrDefault(c => c.Login == username && c.Password == password);
            if (client != null)
            {
                MessageBox.Show("Добро пожаловать, " + client.FullName + "!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                SessionManager.ClientId = client.ClientId;

                OpenClientWindow();
                return;
            }

            // Если пользователь не найден
            MessageBox.Show("Неправильное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OpenUserWindow(int roleId)
        {
            Window targetWindow = null;

            switch (roleId)
            {
                case 1: // Admin
                    targetWindow = new AdminWindow();
                    break;
                case 2: // Manager
                    targetWindow = new ManagerWindow();
                    break;
                default:
                    MessageBox.Show("Роль не распознана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            targetWindow.Show();
            this.Close();
        }

        private void OpenClientWindow()
        {

            ClientWindow a = new ClientWindow();
            a.Show();
            this.Close();

            // Закрываем текущее окно администратора
          
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
