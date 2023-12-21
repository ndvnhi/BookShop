using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Data;
using MyShop.models;
using MyShop.DAO;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<Book> _books;
        List<Category> _categories;
        private Category _selectedCategory;

        public MainWindow()
        {
            InitializeComponent();
            EditBookWindow.BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _rowsPerPage = 5;
            LoadCategories();
            LoadAllBooks();
        }

        int _rowsPerPage = 5;
        int _totalPages = -1;
        int _totalItems = -1;
        int _currentPage = 1;

        private void LoadAllBooks()
        {
            int category_id = -1;
            if (_selectedCategory != null)
            {
                category_id = _selectedCategory.Id;
            }
            decimal min_price = decimal.TryParse(minPriceTextBox.Text, out min_price) ? min_price : 0;
            decimal max_price = decimal.TryParse(maxPriceTextBox.Text, out max_price) ? max_price : decimal.MaxValue;
            string keyword = keywordTextBox.Text;
            var reader = BookDAO.getBooksWithCondition(min_price, max_price, (_currentPage - 1) * _rowsPerPage, _rowsPerPage, keyword, category_id);

            int count = -1;
            _books = new BindingList<Book>();

            using (reader)
            {
                while (reader.Read())
                {
                    var Id = (int)reader["id"];
                    var Name = (string)reader["name"];

                    var Cover_Images = (string)reader["cover_image"];
                    BitmapImage coverImage = new BitmapImage(new Uri(Cover_Images, UriKind.RelativeOrAbsolute));

                    var Author = (string)reader["author"];
                    var Year = (int)reader["year"];
                    var Price = (double)reader["price"];

                    var Category_Id = (int)reader["category_id"];
                    var Category_Name = _categories?.FirstOrDefault(c => c.Id == Category_Id)?.Name;

                    var book = new Book(Id, Name, coverImage, Author, Year, Price, Category_Id, Category_Name);
                    _books.Add(book);

                    count = (int)reader["Total"];
                }
            }

            booksListView.ItemsSource = _books;

            if (count != _totalItems)
            {
                _totalItems = count;

                if (_totalItems < 0)
                {
                    _totalItems = 0;
                }

                _totalPages = (_totalItems > 0) ? (_totalItems / _rowsPerPage) + (((_totalItems % _rowsPerPage) == 0) ? 0 : 1) : 0;

                var pageInfos = new List<object>();

                for (int i = 1; i <= _totalPages; i++)
                {
                    pageInfos.Add(new
                    {
                        Page = i,
                        Total = _totalPages
                    });
                }

                pagingComboBox.ItemsSource = pageInfos;
                pagingComboBox.SelectedIndex = 0;
            }

            totalBooksLabel.Content = $"Displaying {_books.Count} / {_totalItems}";
        }


        // paging
        private void pagingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic info = pagingComboBox.SelectedItem;
            if (info != null)
            {
                if (info?.Page != _currentPage)
                {
                    _currentPage = info?.Page;
                    LoadAllBooks();
                }
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (pagingComboBox.SelectedIndex < pagingComboBox.Items.Count - 1)
            {
                pagingComboBox.SelectedIndex++;
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (pagingComboBox.SelectedIndex > 0)
            {
                pagingComboBox.SelectedIndex--;
            }
        }

        // number of books per page
        private void booksPerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = booksPerPageComboBox.SelectedItem;

            if (selectedItem != null)
            {
                _rowsPerPage = Convert.ToInt32(selectedItem.Content);
                _currentPage = 1;
                LoadAllBooks();
                UpdatePagingInfo();
            }
        }

        private void UpdatePagingInfo()
        {
            if (_totalItems < 0)
            {
                _totalItems = 0;
            }

            _totalPages = (_totalItems > 0) ? (_totalItems / _rowsPerPage) + (((_totalItems % _rowsPerPage) == 0) ? 0 : 1) : 0;

            var pageInfos = new List<object>();

            for (int i = 1; i <= _totalPages; i++)
            {
                pageInfos.Add(new
                {
                    Page = i,
                    Total = _totalPages
                });
            }

            pagingComboBox.ItemsSource = pageInfos;
            pagingComboBox.SelectedIndex = 0;

            totalBooksLabel.Content = $"Displaying {_books.Count} / {_totalItems}";
        }



        // get category
        public List<Category> GetCategoriesFromDatabase()
        {
            List<Category> categories = new List<Category>();

            using (var connection = new SqlConnection(DB.Instance.ConnectionString))
            {
                connection.Open();

                string select_sql = "SELECT id, name FROM Category";
                using (var command = new SqlCommand(select_sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryId = reader.GetInt32(0);
                            string categoryName = reader.GetString(1);

                            categories.Add(new Category { Id = categoryId, Name = categoryName });
                        }
                    }
                }
            }

            return categories;
        }

        // category
        private void LoadCategories()
        {
            _categories = GetCategoriesFromDatabase();

            categoriesComboBox.ItemsSource = _categories;
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = (Category)categoriesComboBox.SelectedItem;
            LoadAllBooks();
        }

        private void ClearComboBoxSelection()
        {
            categoriesComboBox.SelectedItem = null;
        }

        private void ClearSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            ClearComboBoxSelection();
        }


        // add new book
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new AddBookWindow(_books, this);
            addBookWindow.ShowDialog();
            booksListView.Items.Refresh();
            LoadAllBooks();
        }

        // remove a book
        private void RemoveBookFromDatabase(int bookId)
        {
            try
            {
                using (var connection = new SqlConnection(DB.Instance.ConnectionString))
                {
                    connection.Open();

                    string delete_sql = "DELETE FROM MoreBook WHERE id = @Id";
                    using (var command = new SqlCommand(delete_sql, connection))
                    {
                        command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = bookId;

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected <= 0)
                        {
                            MessageBox.Show($"Failed to remove book from the database.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error removing book from the database: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing book from the database: {ex.ToString()}");
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksListView.SelectedItem != null)
            {
                Book selectedBook = (Book)booksListView.SelectedItem;

                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to remove the book with ID '{selectedBook.Id}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    RemoveBookFromDatabase(selectedBook.Id);
                    _books.Remove(selectedBook);
                    MessageBox.Show($"Book with ID '{selectedBook.Id}' removed successfully.");
                    booksListView.Items.Refresh();
                    LoadAllBooks();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to remove.");
            }
        }

        // edit a book
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksListView.SelectedValue != null)
            {
                int selectedBookID = (int)booksListView.SelectedValue;

                Book selectedBook = _books.FirstOrDefault(book => book.Id == selectedBookID);

                if (selectedBook != null)
                {
                    var editBookWindow = new EditBookWindow(selectedBook, this);
                    editBookWindow.ShowDialog();
                }

                booksListView.Items.Refresh();
                LoadAllBooks();
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        // search 
        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadAllBooks();
        }

        // search by price range
        private void searchByPriceButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(minPriceTextBox.Text, out decimal minPrice) &&
                decimal.TryParse(maxPriceTextBox.Text, out decimal maxPrice))
            {
                var filteredBooks = _books.Where(book => (decimal)book.Price >= minPrice && (decimal)book.Price <= maxPrice).ToList();

                booksListView.ItemsSource = filteredBooks;
                LoadAllBooks();

                totalBooksLabel.Content = $"Total Books: {filteredBooks.Count}";
            }
            else
            {
                MessageBox.Show("Invalid price format.");
            }
        }

        private void ClearPriceButton_Click(object sender, RoutedEventArgs e)
        {
            minPriceTextBox.Clear();
            maxPriceTextBox.Clear();
            LoadAllBooks();
        }

        private void OnMinPriceTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Min")
            {
                textBox.Text = "";
            }
        }

        private void OnMaxPriceTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Max")
            {
                textBox.Text = "";
            }
        }

    }
}