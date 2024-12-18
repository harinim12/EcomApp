using EcommerceApplication.Exceptions;
using EcommerceApplication.Model;
using EcommerceApplication.Utility;
using System;
using System.Data.SqlClient;

namespace EcommerceApplication.Repository
{
    public class ProductRepository : IProductRepository
    {
        // Method to create a product


        public bool CreateProduct(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "INSERT INTO Products (name, price, description, stockQuantity) VALUES (@Name, @Price, @Description, @StockQuantity)";
                    SqlCommand command = new SqlCommand(query, connection);

                   
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating product: {ex.Message}");
                return false;
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                   
                    string checkQuery = "SELECT COUNT(1) FROM Products WHERE product_id = @ProductId";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();
                    int productExists = (int)checkCommand.ExecuteScalar();

                    if (productExists == 0)
                    {
                        // Throw exception if the product doesn't exist
                        throw new ProductNotFoundException($"Product with ID {productId} not found.");
                    }

                   
                    string deleteQuery = "DELETE FROM Products WHERE product_id = @ProductId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@ProductId", productId);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    return rowsAffected > 0; 
                }
            }
            catch (ProductNotFoundException ex)
            {
                
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public Product GetProductById(int productId) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "SELECT * FROM Products WHERE product_id = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Product
                        {
                            ProductId = (int)reader["product_id"],
                            Name = reader["name"].ToString(),
                            Price = (decimal)reader["price"],
                            Description = reader["description"].ToString(),
                            StockQuantity = (int)reader["stockQuantity"]
                        };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public bool PlaceOrder(Customer customer, List<Cart> cartItems, string shippingAddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    connection.Open();

                    // Start a transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert into Orders table
                        string insertOrderQuery = @"
                            INSERT INTO Orders (customer_id, total_price, shipping_address, order_date)
                            VALUES (@CustomerId, @TotalPrice, @ShippingAddress, @OrderDate);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand orderCommand = new SqlCommand(insertOrderQuery, connection, transaction);
                        orderCommand.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                        orderCommand.Parameters.AddWithValue("@TotalPrice", cartItems.Sum(c => c.Quantity * c.Price));
                        orderCommand.Parameters.AddWithValue("@ShippingAddress", shippingAddress);
                        orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);

                        int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());


                        foreach (var cartItem in cartItems)
                        {
                            string insertOrderItemQuery = @"
                                INSERT INTO OrderItems (order_id, product_id, quantity, price)
                                VALUES (@OrderId, @ProductId, @Quantity, @Price)";

                            SqlCommand orderItemCommand = new SqlCommand(insertOrderItemQuery, connection, transaction);
                            orderItemCommand.Parameters.AddWithValue("@OrderId", orderId);
                            orderItemCommand.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                            orderItemCommand.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                            orderItemCommand.Parameters.AddWithValue("@Price", cartItem.Price);

                            orderItemCommand.ExecuteNonQuery();

                            
                            string updateStockQuery = @"
                                UPDATE Products
                                SET stockQuantity = stockQuantity - @Quantity
                                WHERE product_id = @ProductId";

                            SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection, transaction);
                            updateStockCommand.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                            updateStockCommand.Parameters.AddWithValue("@ProductId", cartItem.ProductId);

                            updateStockCommand.ExecuteNonQuery();
                        }

                       
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
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

        
    }
}
