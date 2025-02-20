using System.Windows;

namespace WarehouseManagementApp
{
    public partial class EditProductWindow : Window
    {
        private WarehouseDBEntities dbContext;
        private Products product;

        public EditProductWindow(WarehouseDBEntities context, Products selectedProduct)
        {
            InitializeComponent();
            dbContext = context;
            product = selectedProduct;

            // Заполняем поля текущими данными продукта
            NameTextBox.Text = product.Name;
            UnitTextBox.Text = product.UnitOfMeasure;
            BarcodeTextBox.Text = product.Barcode;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные продукта
            product.Name = NameTextBox.Text;
            product.UnitOfMeasure = UnitTextBox.Text;
            product.Barcode = BarcodeTextBox.Text;

            // Сохраняем изменения в базе данных
            dbContext.SaveChanges();

            MessageBox.Show("Товар успешно обновлён!");
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закрываем окно без сохранения
        }
    }
}


