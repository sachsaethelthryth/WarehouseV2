using System;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class AddCategoryWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddCategoryWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаём новую категорию
                var newCategory = new Categories
                {
                    CategoryName = CategoryNameTextBox.Text
                };

                dbContext.Categories.Add(newCategory);
                dbContext.SaveChanges();

                MessageBox.Show("Категория успешно добавлена!");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
