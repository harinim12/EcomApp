using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Model
{
    public class OrderItem
    {
        private int _orderItemId;
        private int _orderId;
        private int _productId;
        private int _quantity;

        // Properties
        public int OrderItemId
        {
            get { return _orderItemId; }
            set { _orderItemId = value; }
        }
        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
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

       
        public OrderItem() { }

        public OrderItem(int orderItemId, int orderId, int productId, int quantity)
        {
            _orderItemId = orderItemId;
            _orderId = orderId;
            _productId = productId;
            _quantity = quantity;
        }

      
        public override string ToString()
        {
            return $"OrderItemId: {_orderItemId}, OrderId: {_orderId}, ProductId: {_productId}, Quantity: {_quantity}";
        }
    }
}

