using Microsoft.Data.SqlClient;
using MyShop.models;
using System;
using System.Collections.Generic;
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
    }
}
