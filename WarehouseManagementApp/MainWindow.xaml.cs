using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class MainWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public MainWindow()
        {
            InitializeComponent();

            dbContext = new WarehouseDBEntities();

            if (!dbContext.Database.Exists())
            {
                MessageBox.Show("База данных недоступна. Проверьте подключение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            LoadAllDataAsync();
            
        }

        private async void LoadAllDataAsync()
        {
            
            await LoadDataAsync(LoadCategories);
            await LoadDataAsync(LoadProducts);
            await LoadDataAsync(LoadCustomers);
            await LoadDataAsync(LoadSuppliers);
            await LoadDataAsync(LoadOperations);
            await LoadDataAsync(LoadRoles);
            await LoadDataAsync(LoadUsers);
            await LoadDataAsync(LoadStockLevels);
            await LoadDataAsync(LoadOperationDetails);
            
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string userRole = Application.Current.Properties["UserRole"] as string;
            if (string.IsNullOrEmpty(userRole))
            {
                MessageBox.Show("Роль пользователя не определена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            switch (userRole)
            {
                case "Administrator":
                    // Полный доступ, ничего не скрываем
                    break;

                case "Manager":
                    // Скрываем вкладку "Пользователи"
                    tabUsers.Visibility = Visibility.Collapsed;
                    break;

                case "WarehouseWorker":
                    // Скрываем подвкладку "Категории" в "Товары"
                    subTabCategories.Visibility = Visibility.Collapsed;
                    // Скрываем подвкладку "Поставщики" в "Операции"
                    subTabSuppliers.Visibility = Visibility.Collapsed;
                    // Скрываем вкладку "Пользователи"
                    tabUsers.Visibility = Visibility.Collapsed;
                    break;

                default:
                    MessageBox.Show("Неизвестная роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    break;
            }
        }

        private async Task LoadDataAsync(Func<Task> loadDataFunc)
        {
            try
            {
                await loadDataFunc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadProducts()
        {
            ProductsDataGrid.ItemsSource = await dbContext.Products.ToListAsync();
        }

        private async Task LoadCustomers()
        {
            CustomersDataGrid.ItemsSource = await dbContext.Customers.ToListAsync();
        }

        private async Task LoadSuppliers()
        {
            SuppliersDataGrid.ItemsSource = await dbContext.Suppliers.ToListAsync();
        }

        private async Task LoadOperations()
        {
            OperationsDataGrid.ItemsSource = await dbContext.WarehouseOperations
                .Include("Users")
                .Include("Suppliers")
                .Include("Customers")
                .ToListAsync();
        }

        private async Task LoadRoles()
        {
            RolesDataGrid.ItemsSource = await dbContext.Roles.ToListAsync();
        }

        private async Task LoadStockLevels()
        {
            StockLevelsDataGrid.ItemsSource = await dbContext.StockLevels.ToListAsync();
        }

        private async Task LoadUsers()
        {
            UsersDataGrid.ItemsSource = await dbContext.Users.ToListAsync();
        }

        private async Task LoadCategories()
        {
            CategoriesDataGrid.ItemsSource = await dbContext.Categories.ToListAsync();
        }

        private async Task LoadOperationDetails()
        {
            OperationDetailsDataGrid.ItemsSource = await dbContext.OperationDetails.ToListAsync();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow(dbContext);
            if (addProductWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadProducts);
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Products selectedProduct)
            {
                var editProductWindow = new EditProductWindow(dbContext, selectedProduct);
                if (editProductWindow.ShowDialog() == true)
                {
                    LoadDataAsync(LoadProducts);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для редактирования.", "Внимание");
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Products selectedProduct)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар \"{selectedProduct.Name}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Products.Remove(selectedProduct);
                    dbContext.SaveChanges();
                    LoadDataAsync(LoadProducts);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Внимание");
            }
        }

        private void RefreshProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadProducts);
        }

        private void RefreshCategories_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadCategories);
        }

        private void RefreshStockLevels_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadStockLevels);
        }

        private void RefreshOperationDetails_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadOperationDetails);
        }

        private void RefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadRoles);
        }

        private void RefreshCustomers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadCustomers);
        }

        private void RefreshSuppliers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadSuppliers);
        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadUsers);
        }

        private void RefreshOperations_Click(object sender, RoutedEventArgs e)
        {
            LoadDataAsync(LoadOperations);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Приложение для автоматизации управления складом продовольственных товаров\nКурсовая работа.\nАвтор: Бильтяев Данила\n Версия:1.0",
                "О программе");
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddCustomerWindow(dbContext);
            if (addCustomerWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadCustomers);
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem is Customers selectedCustomer)
            {
                var editCustomerWindow = new EditCustomerWindow(dbContext, selectedCustomer);
                if (editCustomerWindow.ShowDialog() == true)
                {
                    LoadDataAsync(LoadCustomers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Внимание");
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem is Customers selectedCustomer)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить клиента \"{selectedCustomer.Name}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Customers.Remove(selectedCustomer);
                    dbContext.SaveChanges();
                    LoadDataAsync(LoadCustomers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Внимание");
            }
        }

        private void AddOperation_Click(object sender, RoutedEventArgs e)
        {
            var addOperationWindow = new AddOperationWindow(dbContext);
            if (addOperationWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadOperations);
            }
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            var addSupplierWindow = new AddSupplierWindow(dbContext);
            if (addSupplierWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadSuppliers);
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem is Suppliers selectedSupplier)
            {
                var editSupplierWindow = new EditSupplierWindow(dbContext, selectedSupplier);
                if (editSupplierWindow.ShowDialog() == true)
                {
                    LoadDataAsync(LoadSuppliers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставщика для редактирования.", "Внимание");
            }
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem is Suppliers selectedSupplier)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить поставщика \"{selectedSupplier.Name}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Suppliers.Remove(selectedSupplier);
                    dbContext.SaveChanges();
                    LoadDataAsync(LoadSuppliers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставщика для удаления.", "Внимание");
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(dbContext);
            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadCategories);
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Categories selectedCategory)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить категорию \"{selectedCategory.CategoryName}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Categories.Remove(selectedCategory);
                    dbContext.SaveChanges();
                    LoadDataAsync(LoadCategories);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите категорию для удаления.", "Внимание");
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow(dbContext);
            if (addUserWindow.ShowDialog() == true)
            {
                LoadDataAsync(LoadUsers);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var editUserWindow = new EditUserWindow(dbContext, selectedUser);
                if (editUserWindow.ShowDialog() == true)
                {
                    LoadDataAsync(LoadUsers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.", "Внимание");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить пользователя \"{selectedUser.Username}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Users.Remove(selectedUser);
                    dbContext.SaveChanges();
                    LoadDataAsync(LoadUsers);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Внимание");
            }
        }

        private void SomePlaceholder_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for future functionality
        }
    }
}
