﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyShop.models
{
    public class Book
    {
        public Book() {}

        public Book(int id, string name, string coverImage, string author, int year, double price, int category_Id, string? category_Name, int quantity)
        {
            Id = id;
            Name = name;
            Cover_Image = coverImage;
            Author = author;
            Year = year;
            Price = price;
            Category_Id = category_Id;
            Category_Name = category_Name;
            Quantity = quantity;
        }

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
