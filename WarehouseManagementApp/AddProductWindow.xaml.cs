using System.Linq;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class AddProductWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddProductWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;
            CategoryComboBox.ItemsSource = dbContext.Categories.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Products
            {
                Name = NameTextBox.Text,
                CategoryID = (int)CategoryComboBox.SelectedValue,
                UnitOfMeasure = UnitTextBox.Text,
                Barcode = BarcodeTextBox.Text,
            };

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();
            MessageBox.Show("Товар добавлен!");
            this.DialogResult = true;
        }
    }
}