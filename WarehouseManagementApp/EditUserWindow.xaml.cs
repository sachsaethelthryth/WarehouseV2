using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class EditUserWindow : Window
    {
        private WarehouseDBEntities dbContext;
        private Users user;

        public EditUserWindow(WarehouseDBEntities context, Users selectedUser)
        {
            InitializeComponent();
            dbContext = context;
            user = selectedUser;

            // Загрузка данных пользователя
            UsernameTextBox.Text = user.Username;
            RoleComboBox.ItemsSource = dbContext.Roles.ToList();
            RoleComboBox.SelectedValue = user.RoleID;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем данные пользователя
                user.RoleID = (int)RoleComboBox.SelectedValue;

                // Если введён новый пароль, шифруем его
                if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    user.PasswordHash = HashPasswordToVarbinary(PasswordBox.Password);
                }

                dbContext.SaveChanges();
                MessageBox.Show("Данные пользователя успешно обновлены!");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}");
            }
        }

        // Функция для шифрования пароля
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
