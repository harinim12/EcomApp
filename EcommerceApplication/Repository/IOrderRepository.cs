using EcommerceApplication.Model;
using System;
using System.Collections.Generic;

namespace EcommerceApplication.Repository
{
    public interface IOrderRepository
    {
        bool PlaceOrder(int customerId, List<Cart> cartItems, string shippingAddress);
        List<Order> GetCustomerOrders(int customerId);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);
        

    }
}
