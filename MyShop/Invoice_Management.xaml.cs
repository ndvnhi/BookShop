using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fluent;
using MyShop.DAO;
using MyShop.models;
using System.ComponentModel;
using System.Globalization;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for Invoice_Management.xaml
    /// </summary>
    public class PageInfo
    {
        public int Page { get; set; }
        public int Total { get; set; }
    }
    public partial class Invoice_Management : Fluent.RibbonWindow
    {
        public List<Invoice> _invoices;

        public Invoice_Management()
        {
            InitializeComponent();
            _rowsPerPage = 10;
            LoadInvoices();
        }
        int _rowsPerPage = 10;
        int _totalPages = -1;
        int _totalItems = -1;
        int _currentPage = 1;
        public List<PageInfo> PageList
        {
            get
            {
                return Enumerable.Range(1, _totalPages)
                                 .Select(page => new PageInfo { Page = page, Total = _totalPages })
                                 .ToList();
            }
        }
        private void LoadInvoices()
        {
            try
            {
                _invoices = InvoiceDAO.GetInvoices(DB.Instance.ConnectionString);
                invoiceListView.ItemsSource = _invoices;
                _invoices = InvoiceDAO.GetInvoices(DB.Instance.ConnectionString);
                CalculatePageCount();
                UpdatePageComboBox();
                LoadInvoicesForPage();
                UpdateNavigationButtons();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading invoices: {ex.Message}");
            }
        }

        private void BackstageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RemoveInvoiceFromDatabase(int invoiceId)
        {
            try
            {
                bool success = InvoiceDAO.RemoveInvoice(invoiceId, DB.Instance.ConnectionString);

                if (success)
                {
                    MessageBox.Show($"Invoice with ID '{invoiceId}' removed successfully.");
                }
                else
                {
                    MessageBox.Show($"Failed to remove Invoice with ID '{invoiceId}' from the database.");
                }
            }
            catch (ApplicationException appEx)
            {
                MessageBox.Show($"Error removing invoice from the database: {appEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }
        private void RemoveInvoiceDetailFromDatabase(int invoiceId)
        {
            try
            {
                bool success = InvoiceDAO.RemoveInvoiceDetail(invoiceId, DB.Instance.ConnectionString);

                if (success)
                {
                    MessageBox.Show($"Invoice_Detail with invoiceID '{invoiceId}' removed successfully.");
                }
                else
                {
                    MessageBox.Show($"Failed to remove Invoice_Detail with invoiceID '{invoiceId}' from the database.");
                }
            }
            catch (ApplicationException appEx)
            {
                MessageBox.Show($"Error removing Invoice_Detail from the database: {appEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (invoiceListView.SelectedItem != null)
            {
                Invoice selectedInvoice = (Invoice)invoiceListView.SelectedItem;

                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to remove the Invoice and Invoice Detail with ID '{selectedInvoice.Id}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    RemoveInvoiceDetailFromDatabase(selectedInvoice.Id);
                    RemoveInvoiceFromDatabase(selectedInvoice.Id);
                    _invoices.Remove(selectedInvoice);
                    invoiceListView.Items.Refresh();
                    LoadInvoices();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to remove.");
            }
        }

        private void CalculatePageCount()
        {
            _totalItems = _invoices.Count;
            _totalPages = (_totalItems + _rowsPerPage - 1) / _rowsPerPage;
        }

        private void LoadInvoicesForPage()
        {
            int startIndex = (_currentPage - 1) * _rowsPerPage;
            int endIndex = Math.Min(startIndex + _rowsPerPage, _totalItems);

            List<Invoice> displayedInvoices = _invoices.Skip(startIndex).Take(_rowsPerPage).ToList();
            invoiceListView.ItemsSource = displayedInvoices;
        }
        private void UpdatePageComboBox()
        {
            pageComboBox.ItemsSource = PageList;
            pageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void UpdateNavigationButtons()
        {
            previousButton.IsEnabled = _currentPage > 1;
            nextButton.IsEnabled = _currentPage < _totalPages;
        }

        private void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPage = pageComboBox.SelectedIndex + 1;
            LoadInvoicesForPage();
            UpdateNavigationButtons();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadInvoicesForPage();
                UpdateNavigationButtons();
                pageComboBox.SelectedIndex = _currentPage - 1;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadInvoicesForPage();
                UpdateNavigationButtons();
                pageComboBox.SelectedIndex = _currentPage - 1;
            }
        }
        private void UpdateListView()
        {
            // Cập nhật ItemSource cho ListView
            invoiceListView.ItemsSource = _invoices;

            // Cập nhật ComboBox trang
            UpdatePageComboBox();
        }
        private void ReloadInvoices()
        {
            try
            {
                string keyword = keywordTextBox.Text;

                DateTime dateKeyword;

                // Kiểm tra xem keyword có thể chuyển thành kiểu DateTime hay không
                if (DateTime.TryParse(keyword, out dateKeyword))
                {
                    // Nếu chuyển thành công, tìm kiếm theo ngày
                    _invoices = InvoiceDAO.SearchInvoicesByDate(dateKeyword, DB.Instance.ConnectionString);
                }
                else
                {
                    // Nếu không chuyển thành công, tìm kiếm theo từ khóa
                    _invoices = InvoiceDAO.SearchInvoicesByKeyword(keyword, DB.Instance.ConnectionString);
                }

                // Cập nhật ListView
                UpdateListView();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading invoices: {ex.Message}");
            }
        }

        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadInvoices();

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var addInvoiceWindow = new AddInvoice();
            addInvoiceWindow.ShowDialog();
        }

        private void Home_click(object sender, MouseButtonEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
