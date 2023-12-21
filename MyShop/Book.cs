using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cover_Image { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public int Category_Id { get; set; }
        public int Quantity { get; set; }

        public string Category_Name { get; set; }
    }
}
