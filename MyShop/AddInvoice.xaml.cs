﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyShop.DAO;
using MyShop.models;
using System.ComponentModel;
using System.Globalization;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for AddInvoice.xaml
    /// </summary>
    public partial class AddInvoice : Window
    {
        private Invoice_Management _invoiceManagement;
        private List<Invoice> _invoices;
        public AddInvoice()
        {
            InitializeComponent();
        }
    }
}
