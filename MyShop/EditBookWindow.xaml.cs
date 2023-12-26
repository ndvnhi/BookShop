using Microsoft.Data.SqlClient;
using MyShop.DAO;
using MyShop.models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Xml.Linq;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private Book _currentBook;
        private List<Category> _categories;
        private MainWindow _mainWindow;

        public EditBookWindow(Book book)
        {
            InitializeComponent();
            _currentBook = book;
            //_mainWindow = mainWindow;

            _categories = CategoryDAO.GetCategories(DB.Instance.ConnectionString);
            cmbCategory.ItemsSource = _categories;
            cmbCategory.DisplayMemberPath = "Name";
            cmbCategory.SelectedValuePath = "Id";

            txtName.Text = _currentBook.Name;

            string absoluteImagePath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + _currentBook.Cover_Image;
            txtCover_Image.Text = absoluteImagePath;

            txtAuthor.Text = _currentBook.Author;
            txtYear.Text = _currentBook.Year.ToString();
            txtPrice.Text = _currentBook.Price.ToString();
            cmbCategory.SelectedValue = _currentBook.Category_Id;
            txtQuantity.Text = _currentBook.Quantity.ToString();

            DataContext = _currentBook;
        }


        private string SelectImage()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedImagePath = SelectImage();

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                _currentBook.Cover_Image = selectedImagePath;

                txtCover_Image.Text = selectedImagePath;
            }
        }

        private string GetRelativePath(string absolutePath)
        {
            try
            {
                Uri baseUri = new Uri(BaseDirectory);
                Uri absoluteUri = new Uri(absolutePath);

                Uri relativeUri = baseUri.MakeRelativeUri(absoluteUri);

                return Uri.UnescapeDataString(relativeUri.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting relative path: {ex.Message}");
                return null;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentBook.Name = txtName.Text;

            string coverImage = txtCover_Image.Text;
            if (string.IsNullOrEmpty(coverImage))
            {
                MessageBox.Show("Please select a cover image.");
                return;
            }

            string relativeImagePath = GetRelativePath(coverImage);
            _currentBook.Cover_Image = relativeImagePath;

            _currentBook.Author = txtAuthor.Text;

            if (int.TryParse(txtYear.Text, out int year))
            {
                _currentBook.Year = year;
            }
            else
            {
                MessageBox.Show("Invalid year format.");
                return;
            }

            if (cmbCategory.SelectedItem is Category selectedCategory)
            {
                _currentBook.Category_Id = selectedCategory.Id;
            }
            else
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            decimal price;
            if (decimal.TryParse(txtPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out price))
            {
                _currentBook.Price = (double)Math.Round(price, 2);
            }
            else
            {
                MessageBox.Show("Invalid price format.");
                return;
            }

            if (int.TryParse(txtQuantity.Text, out int quantity))
            {
                _currentBook.Quantity = quantity;
            }
            else
            {
                MessageBox.Show("Invalid quantity format.");
                return;
            }

            UpdateBookInDatabase(_currentBook);

            MessageBox.Show($"Book '{_currentBook.Name}' updated successfully.");
            this.Close();
        }

        private void UpdateBookInDatabase(Book book)
        {
            bool success = BookDAO.UpdateBook(book, DB.Instance.ConnectionString);

            if (!success)
            {
                MessageBox.Show($"Failed to update book '{book.Name}' in the database.");
            }
        }

    }
}
