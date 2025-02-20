using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WarehouseManagementApp
{
    public partial class AddOperationWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddOperationWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;

            // Заполняем выбор пользователя
            UserComboBox.ItemsSource = dbContext.Users.ToList();
            // Заполняем выбор клиента
            CustomerComboBox.ItemsSource = dbContext.Customers.ToList();
            // Заполняем выбор поставщика
            SupplierComboBox.ItemsSource = dbContext.Suppliers.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаём новую операцию
                var newOperation = new WarehouseOperations
                {
                    OperationType = (OperationTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Date = OperationDatePicker.SelectedDate ?? DateTime.Now,
                    UserID = (int)UserComboBox.SelectedValue,
                    CustomerID = CustomerComboBox.SelectedValue as int?,
                    SupplierID = SupplierComboBox.SelectedValue as int?
                };

                dbContext.WarehouseOperations.Add(newOperation);
                dbContext.SaveChanges();

                MessageBox.Show("Операция успешно добавлена!");
                this.DialogResult = true; // Закрываем окно и сигнализируем об успехе
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении операции: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закрыть окно без сохранения
        }
    }
}