using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class LoginWindow : Window
    {
        private WarehouseDBEntities dbContext;

        public LoginWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем логин и пароль с формы
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                // Преобразуем пароль в хэш
                byte[] passwordHash = HashPasswordToVarbinary(password);

                // Проверяем наличие пользователя с таким логином
                var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

                if (user != null && user.PasswordHash.SequenceEqual(passwordHash))
                {
                    // Если логин и пароль совпадают
                    MessageBox.Show($"Добро пожаловать, {user.Username}!", "Вход успешен", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true; // Сигнализируем об успешном входе
                    this.Close();
                }
                else
                {
                    // Если логин или пароль неверны
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            this.DialogResult = false;
            this.Close();
        }

        // Функция для шифрования пароля (SHA-256)
        private byte[] HashPasswordToVarbinary(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}   