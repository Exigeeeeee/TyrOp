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
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }
        private void OpenPersonalPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalPage());
        }

        private void OpenToursPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ToursPage());
        }

        private void OpenOrdersPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }
    }
}

