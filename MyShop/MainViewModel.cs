using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;
using MyShop.models;
using MyShop.DAO;

namespace MyShop.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BindingList<Book> _books;
        public BindingList<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                LoadBooks();
            }
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                GetBookDetails();
            }
        }

        private BitmapImage _bookDetailImage;
        public BitmapImage BookDetailImage
        {
            get { return _bookDetailImage; }
            set
            {
                _bookDetailImage = value;
                OnPropertyChanged(nameof(BookDetailImage));
            }
        }

        private string _bookName;
        public string BookName
        {
            get { return _bookName; }
            set
            {
                _bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private string _bookAuthor;
        public string BookAuthor
        {
            get { return _bookAuthor; }
            set
            {
                _bookAuthor = value;
                OnPropertyChanged(nameof(BookAuthor));
            }
        }

        private string _bookYear;
        public string BookYear
        {
            get { return _bookYear; }
            set
            {
                _bookYear = value;
                OnPropertyChanged(nameof(BookYear));
            }
        }

        private string _bookPrice;
        public string BookPrice
        {
            get { return _bookPrice; }
            set
            {
                _bookPrice = value;
                OnPropertyChanged(nameof(BookPrice));
            }
        }

        private string _bookCategory;
        public string BookCategory
        {
            get { return _bookCategory; }
            set
            {
                _bookCategory = value;
                OnPropertyChanged(nameof(BookCategory));
            }
        }

        private string _bookQuantity;
        public string BookQuantity
        {
            get { return _bookQuantity; }
            set
            {
                _bookQuantity = value;
                OnPropertyChanged(nameof(BookQuantity));
            }
        }

        // Constructor
        public MainViewModel()
        {
            // Initialize properties or perform other setup if needed
        }

        // Load categories and books
        public void LoadCategories()
        {
            Categories = CategoryDAO.GetCategories(DB.Instance.ConnectionString);
        }

        public void LoadBooks()
        {
            try
            {
                int category_id = (SelectedCategory != null) ? SelectedCategory.Id : -1;
                decimal min_price = 0;  
                decimal max_price = decimal.MaxValue;  
                int startIndex = 0;
                int pageSize = 6;  
                string keyword = "";  

                var reader = BookDAO.getBooksWithCondition(min_price, max_price, startIndex, pageSize, keyword, category_id);

                List<Book> books = new List<Book>();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var Id = (int)reader["id"];
                        var Name = (string)reader["name"];
                        var Cover_Images = (string)reader["cover_image"];
                        var Author = (string)reader["author"];
                        var Year = (int)reader["year"];
                        var Price = (double)reader["price"];
                        var Category_Id = (int)reader["category_id"];
                        var Category_Name = _categories?.FirstOrDefault(c => c.Id == Category_Id)?.Name;
                        var Quantity = (int)reader["quantity"];

                        var book = new Book(Id, Name, Cover_Images, Author, Year, Price, Category_Id, Category_Name, Quantity);
                        books.Add(book);
                    }
                }

                Books = new BindingList<Book>(books);

                OnPropertyChanged(nameof(Books));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading books: {ex.Message}");
            }
        }


        // Get details of the selected book
        public void GetBookDetails()
        {
            if (SelectedBook != null)
            {
                BookDetailImage = LoadImage(SelectedBook.Cover_Image); // Assumes Cover_Image is a path or URL
                BookName = $"Name: {SelectedBook.Name}";
                BookAuthor = $"Author: {SelectedBook.Author}";
                BookYear = $"Published Year: {SelectedBook.Year}";
                BookPrice = $"Price: {SelectedBook.Price}";
                BookCategory = $"Category: {SelectedBook.Category_Name}";
                BookQuantity = $"Quantity: {SelectedBook.Quantity}";

                OnPropertyChanged(nameof(BookDetailImage));
                OnPropertyChanged(nameof(BookName));
                OnPropertyChanged(nameof(BookAuthor));
                OnPropertyChanged(nameof(BookYear));
                OnPropertyChanged(nameof(BookPrice));
                OnPropertyChanged(nameof(BookCategory));
                OnPropertyChanged(nameof(BookQuantity));
            }
        }

        // Clear book details
        public void ClearBookDetails()
        {
            SelectedBook = null;
            ClearBookDetailsProperties();
        }

        private void ClearBookDetailsProperties()
        {
            BookDetailImage = null;
            BookName = string.Empty;
            BookAuthor = string.Empty;
            BookYear = string.Empty;
            BookPrice = string.Empty;
            BookCategory = string.Empty;
            BookQuantity = string.Empty;

            OnPropertyChanged(nameof(BookDetailImage));
            OnPropertyChanged(nameof(BookName));
            OnPropertyChanged(nameof(BookAuthor));
            OnPropertyChanged(nameof(BookYear));
            OnPropertyChanged(nameof(BookPrice));
            OnPropertyChanged(nameof(BookCategory));
            OnPropertyChanged(nameof(BookQuantity));
        }

        private BitmapImage LoadImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
            }

            return null;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
