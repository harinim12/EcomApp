using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Model
{
    public class Product
    {
        private int _productId;
        private string _name;
        private decimal _price;
        private string _description;
        private int _stockQuantity;

        // Properties
        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int StockQuantity
        {
            get { return _stockQuantity; }
            set { _stockQuantity = value; }
        }

        public decimal Quantity { get; internal set; }

        // Constructors
        public Product() { }

        public Product(int productId, string name, decimal price, string description, int stockQuantity)
        {
            _productId = productId;
            _name = name;
            _price = price;
            _description = description;
            _stockQuantity = stockQuantity;
        }

        // ToString Method
        public override string ToString()
        {
            return $"ProductId: {_productId}, Name: {_name}, Price: {_price}, Stock: {_stockQuantity}";
        }
    }
}

