using System.Windows;

namespace WarehouseManagementApp
{
    public partial class EditSupplierWindow : Window
    {
        private WarehouseDBEntities dbContext;
        private Suppliers supplier;

        public EditSupplierWindow(WarehouseDBEntities context, Suppliers selectedSupplier)
        {
            InitializeComponent();
            dbContext = context;
            supplier = selectedSupplier;

            // Заполняем поля текущими данными поставщика
            NameTextBox.Text = supplier.Name;
            ContactInfoTextBox.Text = supplier.ContactInfo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные поставщика
            supplier.Name = NameTextBox.Text;
            supplier.ContactInfo = ContactInfoTextBox.Text;

            dbContext.SaveChanges();

            MessageBox.Show("Поставщик успешно обновлён!");
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}