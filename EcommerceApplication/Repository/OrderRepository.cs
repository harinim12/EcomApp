using EcommerceApplication.Model;
using EcommerceApplication.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceApplication.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public bool PlaceOrder(int customerId, List<Cart> cartItems, string shippingAddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert into Orders table
                        string insertOrderQuery = @"INSERT INTO Orders (customer_id, shipping_address, order_date, total_price) 
                                            VALUES (@CustomerId, @ShippingAddress, @OrderDate, @TotalPrice);
                                            SELECT SCOPE_IDENTITY();"; // Retrieve the inserted order_id
                        SqlCommand orderCommand = new SqlCommand(insertOrderQuery, connection, transaction);
                        orderCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
                        orderCommand.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar).Value = shippingAddress;
                        orderCommand.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = DateTime.Now;
                        orderCommand.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = 0;

                        var orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

                        // Insert into OrderItems table for each cart item
                        foreach (var cartItem in cartItems)
                        {
                            string insertOrderItemQuery = @"INSERT INTO OrderItems (order_id, product_id, quantity) 
                                                    VALUES (@OrderId, @ProductId, @Quantity)";
                            SqlCommand orderItemCommand = new SqlCommand(insertOrderItemQuery, connection, transaction);
                            orderItemCommand.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                            orderItemCommand.Parameters.Add("@ProductId", SqlDbType.Int).Value = cartItem.ProductId;
                            orderItemCommand.Parameters.Add("@Quantity", SqlDbType.Int).Value = cartItem.Quantity;
                            //orderItemCommand.Parameters.Add("@Price", SqlDbType.Decimal).Value = cartItem.Price;

                            orderItemCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true; // Order placed successfully
                    }
                    catch
                    {
                        transaction.Rollback(); // Rollback transaction if any error occurs
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while placing order: {ex.Message}");
                return false;
            }
        }


        public List<Order> GetOrdersByCustomer(int customerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "SELECT * FROM Orders WHERE customer_id = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    List<Order> orders = new List<Order>();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            OrderId = (int)reader["order_id"],
                            CustomerId = (int)reader["customer_id"],
                            OrderDate = (DateTime)reader["order_date"],
                            TotalPrice = (decimal)reader["total_price"],
                            ShippingAddress = reader["shipping_address"].ToString()
                        };

                        // Retrieve order items for this order
                        string orderItemsQuery = "SELECT * FROM OrderItems WHERE order_id = @OrderId";
                        SqlCommand orderItemsCommand = new SqlCommand(orderItemsQuery, connection);
                        orderItemsCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
                        SqlDataReader orderItemsReader = orderItemsCommand.ExecuteReader();

                        List<OrderItem> orderItems = new List<OrderItem>();
                        while (orderItemsReader.Read())
                        {
                            orderItems.Add(new OrderItem
                            {
                                OrderItemId = (int)orderItemsReader["order_item_id"],
                                OrderId = (int)orderItemsReader["order_id"],
                                ProductId = (int)orderItemsReader["product_id"],
                                Quantity = (int)orderItemsReader["quantity"]
                            });
                        }

                        order.OrderItems = orderItems;
                        orders.Add(order);
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customer orders: {ex.Message}");
                return null;
            }
        }

        public List<Order> GetCustomerOrders(int customerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    // Query to fetch customer orders
                    string orderQuery = @"
                SELECT order_id, customer_id, order_date, total_price, shipping_address
                FROM Orders
                WHERE customer_id = @CustomerId";

                    SqlCommand orderCommand = new SqlCommand(orderQuery, connection);
                    orderCommand.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    SqlDataReader orderReader = orderCommand.ExecuteReader();

                    List<Order> customerOrders = new List<Order>();

                    while (orderReader.Read())
                    {
                        // Read order details
                        int orderId = orderReader.GetInt32(orderReader.GetOrdinal("order_id"));
                        Order order = new Order
                        {
                            OrderId = orderId,
                            CustomerId = orderReader.GetInt32(orderReader.GetOrdinal("customer_id")),
                            OrderDate = orderReader.GetDateTime(orderReader.GetOrdinal("order_date")),
                            TotalPrice = orderReader.GetDecimal(orderReader.GetOrdinal("total_price")),
                            ShippingAddress = orderReader.GetString(orderReader.GetOrdinal("shipping_address")),
                            OrderItems = GetOrderItemsByOrderId(orderId) // Fetch order items
                        };

                        customerOrders.Add(order);
                    }

                    return customerOrders.Count > 0 ? customerOrders : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer orders: {ex.Message}");
                return null;
            }
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)  // Change to public
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    // Query to fetch order items
                    string orderItemsQuery = @"
            SELECT order_item_id, order_id, product_id, quantity
            FROM OrderItems
            WHERE order_id = @OrderId";

                    SqlCommand orderItemsCommand = new SqlCommand(orderItemsQuery, connection);
                    orderItemsCommand.Parameters.AddWithValue("@OrderId", orderId);

                    connection.Open();
                    SqlDataReader itemsReader = orderItemsCommand.ExecuteReader();

                    List<OrderItem> orderItems = new List<OrderItem>();

                    while (itemsReader.Read())
                    {
                        // Map the OrderItem object
                        OrderItem orderItem = new OrderItem
                        {
                            OrderItemId = itemsReader.GetInt32(itemsReader.GetOrdinal("order_item_id")),
                            OrderId = itemsReader.GetInt32(itemsReader.GetOrdinal("order_id")),
                            ProductId = itemsReader.GetInt32(itemsReader.GetOrdinal("product_id")),
                            Quantity = itemsReader.GetInt32(itemsReader.GetOrdinal("quantity"))
                        };

                        orderItems.Add(orderItem);
                    }

                    return orderItems.Count > 0 ? orderItems : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order items: {ex.Message}");
                return null;
            }
        }


    }
}
