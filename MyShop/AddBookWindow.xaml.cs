using Microsoft.Data.SqlClient;
using MyShop.DAO;
using MyShop.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private BindingList<Book> _books;
        private List<Category> _categories;
        private MainWindow _mainWindow;

        public AddBookWindow(BindingList<Book> books)
        {
            InitializeComponent();
            _books = books;
            //_mainWindow = mainWindow;

            //_categories = mainWindow.GetCategoriesFromDatabase();
            _categories = CategoryDAO.GetCategories(DB.Instance.ConnectionString);
            cmbCategory.ItemsSource = _categories;
            cmbCategory.DisplayMemberPath = "Name";
        }

        // choose image
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
                txtCoverImage.Text = selectedImagePath;
            }
        }

        private string GetRelativePath(string absolutePath)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Uri baseUri = new Uri(baseDirectory);
            Uri absoluteUri = new Uri(absolutePath);

            Uri relativeUri = baseUri.MakeRelativeUri(absoluteUri);

            return Uri.UnescapeDataString(relativeUri.ToString());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtName.Text;
                string author = txtAuthor.Text;
                int year = int.Parse(txtYear.Text);
                decimal price = Math.Round(decimal.Parse(txtPrice.Text, CultureInfo.InvariantCulture.NumberFormat), 2);
                int category = (cmbCategory.SelectedItem as Category)?.Id ?? 0;
                int quantity = int.Parse(txtQuantity.Text);

                string coverImage = txtCoverImage.Text;

                if (string.IsNullOrEmpty(coverImage))
                {
                    MessageBox.Show("Please select a cover image.");
                    return;
                }

                string relativeImagePath = GetRelativePath(coverImage);

                var newBook = new Book()
                {
                    Name = name,
                    Cover_Image = relativeImagePath,
                    Author = author,
                    Year = year,
                    Price = (double)price,
                    Category_Id = category,
                    Quantity = quantity
                };

                int newBookId = BookDAO.AddBook(newBook);

                if (newBookId > 0)
                {
                    newBook.Id = newBookId;
                    _books.Add(newBook);
                    MessageBox.Show($"Insert successful for book: {newBook.Name}, ID: {newBookId}");

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Insert failed. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding book: {ex.Message}");
            }
        }

    }
}

