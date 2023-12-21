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
        public static bool addBook(Book book)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO MoreBook (Name, Cover_Image, Author, Year, Price, Category_Id) " +
                              "VALUES (@Name, @Cover_Image, @Author, @Year, @Price, @Category_Id)";
            cmd.Parameters.AddWithValue("@Name", book.Name);
            cmd.Parameters.AddWithValue("@Cover_Image", book.Cover_Image);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Year", book.Year);
            cmd.Parameters.AddWithValue("@Price", book.Price);
            cmd.Parameters.AddWithValue("@Category_Id", book.Category_Id);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
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
