using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MyShop.models;

namespace MyShop.DAO
{
    internal class BookDAO
    {
        public static int AddBook(Book newBook, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertSql = @"
                        INSERT INTO MoreBook (name, cover_image, author, year, price, category_id, quantity)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @Cover_Image, @Author, @Year, @Price, @Category_Id, @Quantity);
                    ";

                    using (var command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.Add("@Name", System.Data.SqlDbType.Text).Value = newBook.Name;
                        command.Parameters.Add("@Cover_Image", System.Data.SqlDbType.Text).Value = newBook.Cover_Image;
                        command.Parameters.Add("@Author", System.Data.SqlDbType.Text).Value = newBook.Author;
                        command.Parameters.Add("@Year", System.Data.SqlDbType.Int).Value = newBook.Year;
                        command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal).Value = newBook.Price;
                        command.Parameters.Add("@Category_Id", System.Data.SqlDbType.Int).Value = newBook.Category_Id;
                        command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = newBook.Quantity;

                        int newBookId = (int)command.ExecuteScalar();

                        return newBookId;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error adding book: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error adding book: {ex.Message}");
            }
        }

        public static bool UpdateBook(Book updatedBook, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateSql = @"
                    UPDATE MoreBook 
                    SET name = @Name, cover_image = @Cover_Image, author = @Author, year = @Year, price = @Price, category_id = @Category_Id, quantity = @Quantity 
                    WHERE id = @Id";

                    using (var command = new SqlCommand(updateSql, connection))
                    {
                        command.Parameters.Add("@Name", SqlDbType.Text).Value = updatedBook.Name;
                        command.Parameters.Add("@Cover_Image", SqlDbType.Text).Value = updatedBook.Cover_Image;
                        command.Parameters.Add("@Author", SqlDbType.Text).Value = updatedBook.Author;
                        command.Parameters.Add("@Year", SqlDbType.Int).Value = updatedBook.Year;
                        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = updatedBook.Price;
                        command.Parameters.Add("@Category_Id", SqlDbType.Int).Value = updatedBook.Category_Id;
                        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = updatedBook.Quantity;
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = updatedBook.Id;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                throw new ApplicationException($"SQL Error updating book: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new ApplicationException($"Error updating book: {ex.Message}");
            }
        }

        public static bool RemoveBook(int bookId, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteSql = "DELETE FROM MoreBook WHERE id = @Id";
                    using (var command = new SqlCommand(deleteSql, connection))
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = bookId;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error removing book: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error removing book: {ex.Message}");
            }
        }

        public static SqlDataReader getBooksWithCondition(decimal min, decimal max, int skip, int next, string keyword, int category_id)
        {
            SqlCommand cmd;
            if (category_id != -1)
            {
                var sql = @"
                    SELECT *, COUNT(*) OVER() AS Total
                    FROM MoreBook
                    WHERE name LIKE @Keyword 
                        AND category_id = @CategoryId
                        AND price >= @MinPrice AND price <= @MaxPrice

                    ORDER BY id
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

                cmd = new SqlCommand(sql, DB.Instance.Connection);
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = category_id;
            }
            else
            {
                var sql = @"
                    SELECT *, COUNT(*) OVER() AS Total
                    FROM MoreBook
                    WHERE name LIKE @Keyword 
                        AND price >= @MinPrice AND price <= @MaxPrice

                    ORDER BY id
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

                cmd = new SqlCommand(sql, DB.Instance.Connection);
            }

            cmd.Parameters.Add("@Skip", SqlDbType.Int).Value = skip;
            cmd.Parameters.Add("@Take", SqlDbType.Int).Value = next;
            cmd.Parameters.Add("@Keyword", SqlDbType.Text).Value = $"%{keyword}%";

            // for price range
            cmd.Parameters.Add("@MinPrice", SqlDbType.Decimal).Value = min;
            cmd.Parameters.Add("@MaxPrice", SqlDbType.Decimal).Value = max;
            return cmd.ExecuteReader();
        }
    }
}
