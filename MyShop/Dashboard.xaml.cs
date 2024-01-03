using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Fluent;
using MyShop.DAO;
using MyShop.models;
using LiveCharts.Wpf;
using LiveCharts;
using System.Configuration;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Fluent.RibbonWindow
    {
        public Dashboard()
        {
            InitializeComponent();

            // Call the method to load total books when the dashboard is initialized.
            LoadTotalBook();
            LoadAlmostSoldOutBook();
            LoadInStockBookColumnChart();
        }

        BindingList<Book> _books;
        List<Category> _categories;
        int totalBook;
        int almostSoldOutBook;
        // Assuming you have a label named totalBookLabel in your XAML.
        private void LoadTotalBook()
        {
            try
            {
                // Connect to your database and execute a query to get the total number of books.
                int total;

                var reader = BookDAO.getTotalBooks();

                using (reader)
                {
                    while (reader.Read())
                    {
                        total = (int)reader["Total"];
                        totalBook = total;
                    }
                }

                totalBooksLabel.Content = $"{totalBook}";
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or show an error message).
                MessageBox.Show($"Error loading total books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAlmostSoldOutBook()
        {
            try
            {
                // Connect to your database and execute a query to get the total number of books.
                int result;

                var reader = BookDAO.getAlmostSoldOutBooks();

                using (reader)
                {
                    while (reader.Read())
                    {
                        result = (int)reader["Result"];
                        almostSoldOutBook = result;
                    }
                }

                soldOutLabel.Content = $"{almostSoldOutBook}";
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or show an error message).
                MessageBox.Show($"Error loading total books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadInStockBookColumnChart()
        {
            try
            {
                // Connect to your database and execute a query to get the total number of books.
                List<string> categories = new List<string>();
                List<int> quantities = new List<int>();

                var reader = BookDAO.getInStockBooks();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["TheLoai"];
                        int result = (int)reader["SoLuong"];
                        categories.Add(name);
                        quantities.Add(result);
                    }
                }
                ColumnChart.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "In Stock",
                        Values = new ChartValues<int>(quantities)
                    }
                };

                // Labeling the X-axis
                ColumnChart.AxisX.Add(new Axis
                {
                    Title = "Book Category",
                    Labels = categories.ToArray()
                });

                // Labeling the Y-axis
                ColumnChart.AxisY.Add(new Axis
                {
                    Title = "Quantity"
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or show an error message).
                MessageBox.Show($"Error loading total books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackstageTabItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RibbonWindow_Closing(object sender, CancelEventArgs e)
        {
            var lastScreen = this.GetType().Name;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["LastScreen"].Value = lastScreen;
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Home_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
