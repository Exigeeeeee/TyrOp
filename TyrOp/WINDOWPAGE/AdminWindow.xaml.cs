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

namespace TyrOp.WINDOWPAGE
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void ManageUsersButton_Click(object sender, RoutedEventArgs e)
        {
            HomeFrame.Navigate(new ManageClientsPage());
        }

        private void ManageToursButton_Click(object sender, RoutedEventArgs e)
        {
            HomeFrame.Navigate(new ManageToursPage());
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход в раздел 'Отчеты'", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void ManageOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            HomeFrame.Navigate(new OrdersPageAdmin());
        }
    }
}
    

