using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class LoginWindow : Window
    {
        private readonly WarehouseDBEntities dbContext;

        public LoginWindow(WarehouseDBEntities context)
        {
            InitializeComponent();
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем логин и пароль с формы
                string username = UsernameTextBox.Text.Trim();
                string password = PasswordBox.Password;

                // Проверяем, что поля не пустые
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка ввода",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Ищем пользователя синхронно (EF6)
                var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка входа",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Хешируем введенный пароль (без соли, так как PasswordSalt отсутствует)
                byte[] computedHash = HashPassword(password);

                // Сравниваем хеши
                if (computedHash.SequenceEqual(user.PasswordHash))
                {
                    MessageBox.Show($"Добро пожаловать, {user.Username}!", "Вход успешен",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    string userRole = user.Roles.RoleName; // Получаем название роли через навигационное свойство
                    Application.Current.Properties["UserRole"] = userRole;
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка входа",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование, например:
                // Logger.LogError(ex, "Ошибка при входе пользователя {Username}", UsernameTextBox.Text);
                MessageBox.Show("Произошла ошибка при входе. Пожалуйста, попробуйте позже.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Функция для хеширования пароля с использованием SHA-256 (без соли)
        private byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        
    }
}