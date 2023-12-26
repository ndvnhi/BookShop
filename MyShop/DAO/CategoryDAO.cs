using Microsoft.Data.SqlClient;
using MyShop.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    internal class CategoryDAO
    {
        public static List<Category> GetCategories(string connectionString)
        {
            List<Category> categories = new List<Category>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
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
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error retrieving categories: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving categories: {ex.Message}");
            }

            return categories;
        }

        public static int AddCategory(Category newCategory, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertSql = @"
                        INSERT INTO Category (name)
                        OUTPUT INSERTED.Id
                        VALUES (@Name);
                    ";

                    using (var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.Add("@Name", SqlDbType.Text).Value = newCategory.Name;

                        int newCategoryId = (int)command.ExecuteScalar();

                        return newCategoryId;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error adding category: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error adding category: {ex.Message}");
            }
        }

        public static bool UpdateCategory(Category updatedCategory, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateSql = @"
                        UPDATE Category 
                        SET name = @Name
                        WHERE id = @Id
                    ";

                    using (var command = new SqlCommand(updateSql, connection))
                    {
                        command.Parameters.Add("@Name", SqlDbType.Text).Value = updatedCategory.Name;
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = updatedCategory.Id;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error updating category: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating category: {ex.Message}");
            }
        }

        public static bool RemoveCategory(int categoryId, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteSql = "DELETE FROM Category WHERE id = @Id";
                    using (var command = new SqlCommand(deleteSql, connection))
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = categoryId;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error removing category: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error removing category: {ex.Message}");
            }
        }
    }
}
