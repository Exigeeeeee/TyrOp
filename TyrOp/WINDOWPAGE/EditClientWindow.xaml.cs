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
    /// Логика взаимодействия для EditClientWindow.xaml
    /// </summary>
    public partial class EditClientWindow : Window
    {
        private TourOperatorDBEntities _context;
        private  Clients _client;

        public EditClientWindow(Clients client)
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
            _client = client;

            // Заполняем текстовые поля текущими данными клиента
            FullNameBox.Text = _client.FullName;
            EmailBox.Text = _client.Email;
            PhoneBox.Text = _client.Phone;
            AddressBox.Text = _client.Address;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные клиента
            _client.FullName = FullNameBox.Text.Trim();
            _client.Email = EmailBox.Text.Trim();
            _client.Phone = PhoneBox.Text.Trim();
            _client.Address = AddressBox.Text.Trim();

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            MessageBox.Show("Изменения успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true; // Закрыть окно с результатом "успех"
        }
    }
}
