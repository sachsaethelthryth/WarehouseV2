using System.Windows;

namespace WarehouseManagementApp
{
    public partial class AddCustomerWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddCustomerWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового клиента
            var newCustomer = new Customers
            {
                Name = NameTextBox.Text,
                ContactInfo = ContactInfoTextBox.Text
            };

            dbContext.Customers.Add(newCustomer);
            dbContext.SaveChanges();

            MessageBox.Show("Клиент успешно добавлен!");
            this.DialogResult = true; // Закрыть окно и вернуть положительный результат
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закрыть окно без сохранения
        }
    }
}