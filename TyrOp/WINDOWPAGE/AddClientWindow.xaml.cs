using System;
using System.Linq;
using System.Windows;
using TyrOp.Model;

namespace TyrOp.WINDOWPAGE
{
    public partial class AddClientWindow : Window
    {
        private readonly TourOperatorDBEntities _context;
        private readonly Clients _existingClient; // Храним клиента для редактирования

        public AddClientWindow()
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
        }

        public AddClientWindow(Clients existingClient) : this()
        {
            _existingClient = existingClient; // Сохраняем клиента для дальнейшей работы

            if (_existingClient != null)
            {
                // Заполняем поля для редактирования
                FullNameBox.Text = _existingClient.FullName;
                EmailBox.Text = _existingClient.Email;
                PhoneBox.Text = _existingClient.Phone;
                AddressBox.Text = _existingClient.Address;
                LoginBox.Text = _existingClient.Login;
                PasswordBox.Text = _existingClient.Password;

                // Блокируем редактирование логина (если это ключевое поле)
                LoginBox.IsEnabled = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (ФИО, Email, Телефон).",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_existingClient != null)
                {
                    // Обновляем существующего клиента
                    _existingClient.FullName = FullNameBox.Text.Trim();
                    _existingClient.Email = EmailBox.Text.Trim();
                    _existingClient.Phone = PhoneBox.Text.Trim();
                    _existingClient.Address = AddressBox.Text.Trim();
                    _existingClient.Password = PasswordBox.Text.Trim();
                }
                else
                {
                    // Проверяем уникальность Login перед добавлением нового клиента
                    var duplicateLogin = _context.Clients.FirstOrDefault(c => c.Login == LoginBox.Text.Trim());
                    if (duplicateLogin != null)
                    {
                        MessageBox.Show("Клиент с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Создаем нового клиента
                    var newClient = new Clients
                    {
                        FullName = FullNameBox.Text.Trim(),
                        Email = EmailBox.Text.Trim(),
                        Phone = PhoneBox.Text.Trim(),
                        Address = AddressBox.Text.Trim(),
                        Login = LoginBox.Text.Trim(),
                        Password = PasswordBox.Text.Trim()
                    };

                    _context.Clients.Add(newClient);
                }

                // Сохраняем изменения в базе данных
                _context.SaveChanges();
                MessageBox.Show("Клиент успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; // Закрываем окно
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                // Обработка ошибок базы данных
                var innerException = ex.InnerException?.InnerException;
                if (innerException != null)
                {
                    MessageBox.Show($"Ошибка базы данных: {innerException.Message}",
                                    "Ошибка обновления",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Ошибка базы данных: {ex.Message}",
                                    "Ошибка обновления",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
