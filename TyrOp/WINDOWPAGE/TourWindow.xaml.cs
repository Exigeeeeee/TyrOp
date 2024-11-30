using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Linq;
using TyrOp.Model;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TyrOp.WINDOWPAGE
{
    public partial class TourWindow : Window
    {
        private readonly TourOperatorDBEntities _context;
        private readonly Tours _tour;

        // Конструктор для добавления нового тура
        public TourWindow()
        {
            InitializeComponent();
            _context = TourOperatorDBEntities.GetContext();

            // Инициализация ComboBox с пользователями
            ResponsiblePersonComboBox.ItemsSource = _context.Users.ToList();
            ResponsiblePersonComboBox.DisplayMemberPath = "UserName";
            ResponsiblePersonComboBox.SelectedValuePath = "UserId";

            // Значения по умолчанию
            StartDatePicker.SelectedDate = DateTime.Now;
            EndDatePicker.SelectedDate = DateTime.Now.AddDays(7);
            StatusComboBox.SelectedIndex = 0; // "Active" по умолчанию
        }

        public TourWindow(Tours tour) : this()
        {
            _tour = tour;

            // Получаем уникальные значения статусов из таблицы Tours
            var statuses = _context.Tours
                                   .Where(t => t.Status != null)
                                   .Select(t => t.Status)
                                   .Distinct()
                                   .ToList();

            StatusComboBox.ItemsSource = statuses;

            if (statuses.Contains(_tour.Status))
            {
                StatusComboBox.SelectedItem = _tour.Status;
            }

            // Заполняем другие поля
            TourNameBox.Text = _tour.TourName;
            DescriptionBox.Text = _tour.Description;
            StartDatePicker.SelectedDate = _tour.StartDate;
            EndDatePicker.SelectedDate = _tour.EndDate;
            ResponsiblePersonComboBox.SelectedValue = _tour.ResponsiblePersonId;

            // Отображение существующего изображения
            if (!string.IsNullOrWhiteSpace(_tour.ImagePath) && File.Exists(_tour.ImagePath))
            {
                ImagePathBox.Text = _tour.ImagePath;
                TourImage.Source = new BitmapImage(new Uri(_tour.ImagePath));
            }
        }

        // Обработчик выбора изображения
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
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
                    TourImage.Source = new BitmapImage(new Uri(selectedFilePath));
                }
                else
                {
                    MessageBox.Show("Выбранный файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Обработчик сохранения данных
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ResponsiblePersonComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите ответственное лицо для тура.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int responsiblePersonId = (int)ResponsiblePersonComboBox.SelectedValue;

                if (_tour == null) // Режим добавления
                {
                    var newTour = new Tours
                    {
                        TourName = TourNameBox.Text.Trim(),
                        Description = DescriptionBox.Text.Trim(),
                        StartDate = StartDatePicker.SelectedDate ?? DateTime.Now,
                        EndDate = EndDatePicker.SelectedDate ?? DateTime.Now.AddDays(7),
                        Status = (string)StatusComboBox.SelectedItem,
                        ResponsiblePersonId = responsiblePersonId,
                        ImagePath = ImagePathBox.Text
                    };

                    _context.Tours.Add(newTour);
                }
                else // Режим редактирования
                {
                    _tour.TourName = TourNameBox.Text.Trim();
                    _tour.Description = DescriptionBox.Text.Trim();
                    _tour.StartDate = StartDatePicker.SelectedDate ?? _tour.StartDate;
                    _tour.EndDate = EndDatePicker.SelectedDate ?? _tour.EndDate;
                    _tour.Status = (string)StatusComboBox.SelectedItem;
                    _tour.ResponsiblePersonId = responsiblePersonId;

                    // Обновляем путь к изображению, если он изменился
                    if (!string.Equals(_tour.ImagePath, ImagePathBox.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        _tour.ImagePath = ImagePathBox.Text;
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Тур успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
