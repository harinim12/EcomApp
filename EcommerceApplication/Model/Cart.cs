using System;

namespace EcommerceApplication.Model
{
    public class Cart
    {
        private int _cartId;
        private int _customerId;
        private int _productId;
        private int _quantity;
        private string _productName; 
        private decimal _price; 
        private decimal _totalPrice;

     
        public int CartId
        {
            get { return _cartId; }
            set { _cartId = value; }
        }
        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }

       
        public Cart()
        {
            _productName = null;
            _price = 0;
            _totalPrice = 0;
        }

        public Cart(int cartId, int customerId, int productId, int quantity)
        {
            _cartId = cartId;
            _customerId = customerId;
            _productId = productId;
            _quantity = quantity;
            _productName = null; 
            _price = 0; 
            _totalPrice = 0; 
        }

       
        public override string ToString()
        {
            return $"CartId: {_cartId}, CustomerId: {_customerId}, ProductId: {_productId}, Quantity: {_quantity}, ProductName: {_productName}, Price: {_price}, TotalPrice: {_totalPrice}";
        }
    }
}
