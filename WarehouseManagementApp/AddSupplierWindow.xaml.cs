using System.Windows;

namespace WarehouseManagementApp
{
    public partial class AddSupplierWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddSupplierWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового поставщика
            var newSupplier = new Suppliers
            {
                Name = NameTextBox.Text,
                ContactInfo = ContactInfoTextBox.Text
            };

            dbContext.Suppliers.Add(newSupplier);
            dbContext.SaveChanges();

            MessageBox.Show("Поставщик успешно добавлен!");
            this.DialogResult = true; // Закрыть окно и вернуть результат
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закрыть окно без сохранения
        }
    }
}