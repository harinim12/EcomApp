using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Model
{
    public class Order
    {
        private int _orderId;
        private int _customerId;
        private DateTime _orderDate;
        private decimal _totalPrice=0;
        private string _shippingAddress;

      
        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }
        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public string ShippingAddress
        {
            get { return _shippingAddress; }
            set { _shippingAddress = value; }
        }

        public List<OrderItem> OrderItems { get; internal set; }

        public Order() { }

        public Order(int orderId, int customerId, DateTime orderDate, decimal totalPrice, string shippingAddress)
        {
            _orderId = orderId;
            _customerId = customerId;
            _orderDate = orderDate;
            _totalPrice = totalPrice;
            _shippingAddress = shippingAddress;
        }

        
        public override string ToString()
        {
            return $"OrderId: {_orderId}, CustomerId: {_customerId}, TotalPrice: {_totalPrice}, ShippingAddress: {_shippingAddress}";
        }
    }
}

