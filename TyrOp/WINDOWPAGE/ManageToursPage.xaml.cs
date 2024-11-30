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
    /// Логика взаимодействия для ManageToursPage.xaml
    /// </summary>
    public partial class ManageToursPage : Page
    {
        private readonly TourOperatorDBEntities _context;

        public ManageToursPage()
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();
            LoadTours();
        }

        /// <summary>
        /// Загружает список туров в DataGrid.
        /// </summary>
        private void LoadTours()
        {
            // Загрузка данных из базы
            var tours = _context.Tours.ToList();
            ToursDataGrid.ItemsSource = tours;
        }

        /// <summary>
        /// Открывает окно для добавления нового тура.
        /// </summary>
        private void AddTourButton_Click(object sender, RoutedEventArgs e)
        {
            var tourWindow = new TourWindow(); // Новый тур
            if (tourWindow.ShowDialog() == true)
            {
                LoadTours(); // Обновляем список туров
            }
        }

        /// <summary>
        /// Открывает окно для редактирования выбранного тура.
        /// </summary>
        private void EditTourButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTour = (Tours)ToursDataGrid.SelectedItem;
            if (selectedTour == null)
            {
                MessageBox.Show("Выберите тур для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tourWindow = new TourWindow(selectedTour); // Передаем выбранный тур
            if (tourWindow.ShowDialog() == true)
            {
                LoadTours(); // Обновляем список туров
            }
        }

        /// <summary>
        /// Удаляет выбранный тур после подтверждения.
        /// </summary>
        private void DeleteTourButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранного тура
            var selectedTour = (Tours)ToursDataGrid.SelectedItem;
            if (selectedTour == null)
            {
                MessageBox.Show("Выберите тур для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтверждение удаления
            if (MessageBox.Show($"Вы уверены, что хотите удалить тур '{selectedTour.TourName}'?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Tours.Remove(selectedTour); // Удаление
                    _context.SaveChanges();
                    LoadTours(); // Перезагрузка списка
                    MessageBox.Show("Тур успешно удален.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}