using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var lastScreen = ConfigurationManager.AppSettings["LastScreen"];
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            if (password.Length != 0)
            {
                var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];

                var cyperTextInBytes = Convert.FromBase64String(password);
                var entropyInBytes = Convert.FromBase64String(entropyIn64);

                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                password = Encoding.UTF8.GetString(passwordInBytes);
            }
            if (lastScreen != "" && username != "" && password != "")
            {
                string connectionString = LoginWindow.connString(username, password);
                DB.Instance.ConnectionString = connectionString;
                LoginWindow.connection = new SqlConnection(connectionString);
                LoginWindow.connection.OpenAsync();

                switch (lastScreen)
                {
                    case "MainWindow":
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        break;

                    case "Dashboard":
                        var dashboard = new Dashboard();
                        dashboard.Show();
                        break;

                    default:
                        var loginWindow = new LoginWindow();
                        loginWindow.Show();
                        break;
                }
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        }

    }

}
