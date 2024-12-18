using EcommerceApplication.Exceptions;
using EcommerceApplication.Model;
using EcommerceApplication.Utility;
using System;
using System.Data.SqlClient;
using System.Text;

namespace EcommerceApplication.Repository
{
    public class CartRepository : ICartRepository
    {
        public bool AddToCart(int customerId, int productId, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    // Check if the product exists in the Product table
                    string productQuery = "SELECT COUNT(1) FROM Products WHERE product_id = @ProductId";
                    SqlCommand productCommand = new SqlCommand(productQuery, connection);
                    productCommand.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();
                    int productExists = (int)productCommand.ExecuteScalar();

                    if (productExists == 0)
                    {
                        throw new ProductNotFoundException($"Product with ID {productId} not found.");
                    }

                    // Check if the customer exists in the Customers table
                    string customerQuery = "SELECT COUNT(1) FROM Customers WHERE customer_id = @CustomerId";
                    SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
                    customerCommand.Parameters.AddWithValue("@CustomerId", customerId);

                    int customerExists = (int)customerCommand.ExecuteScalar();

                    if (customerExists == 0)
                    {
                        throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                    }

                   
                    string stockQuery = "SELECT stockQuantity FROM Products WHERE product_id = @ProductId";
                    SqlCommand stockCommand = new SqlCommand(stockQuery, connection);
                    stockCommand.Parameters.AddWithValue("@ProductId", productId);

                    int stockQuantity = (int)stockCommand.ExecuteScalar();
                    if (quantity > stockQuantity)
                    {
                        Console.WriteLine($"Insufficient stock. Only {stockQuantity} items available.");
                        return false;
                    }

                    
                    string query = "INSERT INTO Cart (customer_id, product_id, quantity) VALUES (@CustomerId, @ProductId, @Quantity)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (CustomerNotFoundException ex)
            {
                // Handle customer not found exception
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            catch (ProductNotFoundException ex)
            {
                // Handle product not found exception
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public List<Cart> GetCartItemsByCustomerId(int customerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                   
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT c.product_id, p.name AS product_name, c.quantity, p.price ");
                    queryBuilder.Append("FROM Cart c ");
                    queryBuilder.Append("JOIN Products p ON c.product_id = p.product_id ");
                    queryBuilder.Append("WHERE c.customer_id = @CustomerId");

                    SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    List<Cart> cartItems = new List<Cart>();
                    while (reader.Read())
                    {
                        try
                        {
                           
                            int quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                            decimal price = reader.GetDecimal(reader.GetOrdinal("price"));
                            decimal totalPrice = quantity * price;

                          
                            Cart cartItem = new Cart
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                                ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                                Quantity = quantity,
                                Price = price, 
                                TotalPrice = totalPrice
                            };

                            cartItems.Add(cartItem);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing cart item: {ex.Message}");
                            continue; 
                        }
                    }

                    return cartItems.Count > 0 ? cartItems : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cart items: {ex.Message}");
                return null;
            }
        }


        public void ClearCart(int customerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "DELETE FROM Cart WHERE customer_id = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
            }
        }



    }
}
