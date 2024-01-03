using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.models
{
    public class Invoice
    {
        public Invoice() { }
        public Invoice(int id, string customer_name, string address, string phone, DateTime created_date, float totalPrice) {
            Id = id;
            CustomerName = customer_name;
            Address = address;
            Phone = phone;
            Created_date = created_date;
            TotalPrice = totalPrice;
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Created_date { get; set; }
        public float TotalPrice { get; set; }
    }
}
