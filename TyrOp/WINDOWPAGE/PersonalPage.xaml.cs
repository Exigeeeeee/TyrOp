using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TyrOp.Model;

namespace TyrOp.WINDOWPAGE
{
    public partial class PersonalPage : Page
    {
        private readonly TourOperatorDBEntities _dbContext;
        private readonly int _clientId;

        public PersonalPage()
        {
            InitializeComponent();
            _dbContext = new TourOperatorDBEntities();

            if (SessionManager.ClientId != null)
            {
                _clientId = SessionManager.ClientId.Value;
                LoadClientData();
            }
            else
            {
                MessageBox.Show("Ошибка: пользователь не авторизован.");
            }
        }

        void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog
            {
                Filter = "Файлы изображений: (*.png, *jpg, *jpeg)|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Pictures"
            };

            if (GetImageDialog.ShowDialog() == true)
            {
                string selectedFilePath = GetImageDialog.FileName;

                if (File.Exists(selectedFilePath))
                {
                    ImagePathBox.Text = selectedFilePath;
                    ClientImage.Source = new BitmapImage(new Uri(selectedFilePath));
                }
                else
                {
                    MessageBox.Show("Выбранный файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        void LoadClientData()
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.ClientId == _clientId);
            if (client != null)
            {
                FullNameTextBox.Text = client.FullName;
                EmailTextBox.Text = client.Email;
                PhoneTextBox.Text = client.Phone;
                LoginTextBox.Text = client.Login;
                PasswordBox.Password = client.Password;

                if (!string.IsNullOrWhiteSpace(client.ImagePath) && File.Exists(client.ImagePath))
                {
                    ClientImage.Source = new BitmapImage(new Uri(client.ImagePath));
                }
            }
        }

        void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.ClientId == _clientId);
            if (client != null)
            {
                client.FullName = FullNameTextBox.Text;
                client.Email = EmailTextBox.Text;
                client.Phone = PhoneTextBox.Text;
                client.Login = LoginTextBox.Text;
                client.Password = PasswordBox.Password;

                if (!string.IsNullOrWhiteSpace(ImagePathBox.Text))
                {
                    client.ImagePath = ImagePathBox.Text;
                }

                _dbContext.SaveChanges();
                MessageBox.Show("Данные успешно обновлены.");
            }
        }
    }
}
