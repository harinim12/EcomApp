using EcommerceApplication.Model;
using EcommerceApplication.Utility;
using System.Data.SqlClient;

namespace EcommerceApplication.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public bool RegisterCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "INSERT INTO customers (name, email, password) VALUES (@Name, @Email, @Password)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Password", customer.Password);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public Customer GetCustomerById(int customerId) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnectionString()))
                {
                    string query = "SELECT * FROM Customers WHERE customer_id = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerId = (int)reader["customer_id"],
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString(),
                            Password = reader["password"].ToString()
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
    }
}
