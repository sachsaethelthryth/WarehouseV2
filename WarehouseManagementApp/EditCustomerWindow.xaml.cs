using System.Windows;

namespace WarehouseManagementApp
{
    public partial class EditCustomerWindow : Window
    {
        private WarehouseDBEntities dbContext;
        private Customers customer;

        public EditCustomerWindow(WarehouseDBEntities context, Customers selectedCustomer)
        {
            InitializeComponent();
            dbContext = context;
            customer = selectedCustomer;

            // Заполняем поля текущими данными клиента
            NameTextBox.Text = customer.Name;
            ContactInfoTextBox.Text = customer.ContactInfo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные клиента
            customer.Name = NameTextBox.Text;
            customer.ContactInfo = ContactInfoTextBox.Text;

            dbContext.SaveChanges();

            MessageBox.Show("Клиент успешно обновлён!");
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}