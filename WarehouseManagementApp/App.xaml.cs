using System.Windows;

namespace WarehouseManagementApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var dbContext = new WarehouseDBEntities();
            var loginWindow = new LoginWindow(dbContext);
            if (loginWindow.ShowDialog() == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}