using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MyShop.models;
namespace MyShop.DAO
{
    internal class InvoiceDAO
    {
        public static List<Invoice> GetInvoices(string connectionString)
        {
            List<Invoice> invoices = new List<Invoice>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string select_sql = "SELECT id, customer_name, address, phone, created_date, totalPrice FROM Invoice";
                    using (var command = new SqlCommand(select_sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader["id"];
                                string customerName = (string)reader["customer_name"];
                                string address = (string)reader["address"];
                                string phone = (string)reader["phone"];
                                DateTime createdDate = (DateTime)reader["created_date"];
                                float totalPrice = (float)Convert.ToDecimal(reader["totalPrice"]);
                                Console.WriteLine($"Name:{customerName}");
                                invoices.Add(new Invoice { Id = id, CustomerName = customerName, Address = address, Phone= phone, Created_date = createdDate, TotalPrice= totalPrice });
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

            return invoices;
        }
        public static bool RemoveInvoice(int invoiceId, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteSql = "DELETE FROM Invoice WHERE id = @Id";
                    using (var command = new SqlCommand(deleteSql, connection))
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = invoiceId;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error removing invoice: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error removing invoice: {ex.Message}");
            }
        }
        public static bool RemoveInvoiceDetail(int invoiceId, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteSql = "DELETE FROM Invoice_Detail WHERE invoice_id = @Id";
                    using (var command = new SqlCommand(deleteSql, connection))
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = invoiceId;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException($"SQL Error removing invoice_detail: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error removing invoice_detail: {ex.Message}");
            }
        }
        public static List<Invoice> SearchInvoicesByKeyword(string keyword, string connectionString)
        {
            List<Invoice> invoices = new List<Invoice>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Sử dụng câu truy vấn SQL với điều kiện tìm kiếm
                    string select_sql = $"SELECT * FROM Invoice WHERE Created_date LIKE @keyword";

                    using (var command = new SqlCommand(select_sql, connection))
                    {
                        // Thêm tham số cho câu truy vấn
                        command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string customerName = reader.GetString(1);
                                string address = reader.GetString(2);
                                string phone = reader.GetString(3);
                                DateTime created_date = reader.GetDateTime(4);
                                float totalPrice = reader.GetFloat(5);

                                invoices.Add(new Invoice { Id = id, CustomerName = customerName, Address = address, Phone = phone, Created_date = created_date, TotalPrice = totalPrice });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error searching invoices: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching invoices: {ex.Message}");
            }

            return invoices;
        }


        public static List<Invoice> SearchInvoicesByDate(DateTime dateKeyword, string connectionString)
        {
            List<Invoice> invoices = new List<Invoice>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string select_sql = "SELECT * FROM Invoice WHERE Created_date = @dateKeyword";

                    using (var command = new SqlCommand(select_sql, connection))
                    {
                        command.Parameters.AddWithValue("@dateKeyword", dateKeyword);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string customerName = reader.GetString(1);
                                string address = reader.GetString(2);
                                string phone = reader.GetString(3);
                                DateTime created_date = reader.GetDateTime(4);
                                float totalPrice = reader.GetFloat(5);

                                invoices.Add(new Invoice { Id = id, CustomerName = customerName, Address = address, Phone = phone, Created_date = created_date, TotalPrice = totalPrice });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error searching invoices by date: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching invoices by date: {ex.Message}");
            }

            return invoices;
        }

    }
}
