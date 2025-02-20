using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class AddUserWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public AddUserWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;

            // Заполнение ComboBox ролями
            RoleComboBox.ItemsSource = dbContext.Roles.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем значения с формы
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;
                int roleId = (int)RoleComboBox.SelectedValue;

                // Преобразуем пароль в varbinary(64)
                byte[] passwordHash = HashPasswordToVarbinary(password);

                // Создаём нового пользователя
                var newUser = new Users
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    RoleID = roleId
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                MessageBox.Show("Пользователь успешно добавлен!");
                this.DialogResult = true; // Закрываем окно
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}");
            }
        }

        // Функция для шифрования пароля в varbinary(64)
        private byte[] HashPasswordToVarbinary(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
