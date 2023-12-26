using System;
using System.Collections.Generic;
using System.Windows;
using MyShop.models;
using MyShop.DAO;

namespace MyShop
{
    public partial class CategoryWindow : Window
    {
        private List<Category> _categories;
        private Category _selectedCategory;

        public CategoryWindow()
        {
            InitializeComponent();

            LoadCategories();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                _categories = CategoryDAO.GetCategories(DB.Instance.ConnectionString);

                categoryListView.ItemsSource = _categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void categoriesListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedCategory = (Category)categoryListView.SelectedItem;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = categoryNameTextBox.Text;

            if (!string.IsNullOrEmpty(categoryName))
            {
                Category newCategory = new Category
                {
                    Name = categoryName
                };

                try
                {
                    int newCategoryId = CategoryDAO.AddCategory(newCategory, DB.Instance.ConnectionString);

                    MessageBox.Show($"Category added with ID: {newCategoryId}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a category name.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (categoryListView.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete the category '{_selectedCategory.Name}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool deletionSuccessful = CategoryDAO.RemoveCategory(_selectedCategory.Id, DB.Instance.ConnectionString);

                        if (deletionSuccessful)
                        {
                            MessageBox.Show($"Category '{_selectedCategory.Name}' deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // After deleting, refresh the categories list
                            LoadCategories();
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete the category '{_selectedCategory.Name}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategory = (Category)categoryListView.SelectedItem;

            if (_selectedCategory != null)
            {
                string updatedCategoryName = categoryNameTextBox.Text;

                if (!string.IsNullOrEmpty(updatedCategoryName))
                {
                    Category updatedCategory = new Category
                    {
                        Id = _selectedCategory.Id,
                        Name = updatedCategoryName
                    };

                    try
                    {
                        bool updateSuccessful = CategoryDAO.UpdateCategory(updatedCategory, DB.Instance.ConnectionString);

                        if (updateSuccessful)
                        {
                            MessageBox.Show($"Category '{_selectedCategory.Name}' updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadCategories();
                        }
                        else
                        {
                            MessageBox.Show($"Failed to update the category '{_selectedCategory.Name}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a category name.");
                }
            }
            else
            {
                MessageBox.Show("Please select a category to edit.");
            }
        }
    }
}
