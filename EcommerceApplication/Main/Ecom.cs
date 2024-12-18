using EcommerceApplication.Exceptions;
using EcommerceApplication.Model;
using EcommerceApplication.Repository;
using System;
using System.Collections.Generic;

namespace EcommerceApplication.Main
{
    public static class Ecom
    {
       
        private static readonly ICustomerRepository customerRepo = new CustomerRepository();
        private static readonly IProductRepository productRepo = new ProductRepository();  
        private static readonly ICartRepository cartRepo = new CartRepository();
        private static readonly IOrderRepository orderRepo = new OrderRepository();

      
        public static void Start()
        {
            bool exit = false;

            while (!exit)
            {
               
                Console.Clear();
                Console.WriteLine("*************************************************************************");
                Console.WriteLine("----------------------WELCOME TO ECOMMERCE APPLICATION-------------------");
                Console.WriteLine("*************************************************************************");

                Console.WriteLine("\t1. Register Customer\n");
                Console.WriteLine("\t2. Create Product\n");
                Console.WriteLine("\t3. Delete Product\n");
                Console.WriteLine("\t4. Add to Cart\n");
                Console.WriteLine("\t5. View Cart\n");
                Console.WriteLine("\t6. Place Order\n");
                Console.WriteLine("\t7. View Customer Orders\n");
                Console.WriteLine("\t8. Exit\n");
                Console.WriteLine();
                Console.Write("Enter 1 - 8 : ");

              
                int choice = int.Parse(Console.ReadLine());

             
                switch (choice)
                {
                    case 1:
                        CreateCustomer();
                        break;

                    case 2:
                        CreateProduct();
                        break;

                    case 3:
                        DeleteProduct();
                        break;

                    case 4:
                        AddToCart();
                        break;

                    case 5:
                        ViewCart();
                        break;

                    case 6:
                        PlaceOrder();
                        break;

                    case 7:
                        ViewCustomerOrders();
                        break;

                    case 8:
                     
                        Console.WriteLine("THANK YOU, BUY AGAIN");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Please choose between 1 and 8.");
                        break;
                }

               
                Console.WriteLine("\nPress any key to cont...");
                Console.ReadKey();
            }
        }

       
        private static void CreateCustomer()
        {
            
            try
            {
                Console.Clear();
                Console.WriteLine("** Register Customer **");

              
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name cannot be empty or null.");
                    return;
                }

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email cannot be empty or null.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty or null.");
                    return;
                }

              
                Customer newCustomer = new Customer
                {
                    Name = name,
                    Email = email,
                    Password = password
                };

                // Call repository method to add customer to database
                bool result = customerRepo.RegisterCustomer(newCustomer);

                if (result)
                {
                    Console.WriteLine("\nCustomer registered successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to register customer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }

          
            Console.WriteLine("\nPress any key ");
            Console.ReadKey();
        }

        private static void CreateProduct()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("**Create Product **");

           
                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Product Name cannot be empty or null.");
                    return;
                }

                Console.Write("Enter Product Description: ");
                string productDescription = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(productDescription))
                {
                    Console.WriteLine("Product Description cannot be empty or null.");
                    return;
                }

                Console.Write("Enter Product Price: ");
                decimal price;
                if (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                {
                    Console.WriteLine("Invalid price. Please enter a positive number.");
                    return;
                }

                Console.Write("Enter Product Quantity: ");
                int quantity;
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity. Please enter a positive number.");
                    return;
                }

                Product newProduct = new Product
                {
                    Name = productName,
                    Description = productDescription,
                    Price = price,
                    StockQuantity = quantity
                };

               
                bool result = productRepo.CreateProduct(newProduct);

                if (result)
                {
                    Console.WriteLine("\nProduct created successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to create product.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }

         
            Console.WriteLine("\nPress any key ...");
            Console.ReadKey();
        }

        
        private static void DeleteProduct()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("**Delete Product **");

                Console.Write("Enter Product ID to delete: ");
                int productId = int.Parse(Console.ReadLine());

                bool result = productRepo.DeleteProduct(productId);

                if (result)
                {
                    Console.WriteLine("Product deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete product.");
                }
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); 
            }

            Console.WriteLine("\nPress any key ..");
            Console.ReadKey();
        }



        private static void AddToCart()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("**Add to Cart **");

               
                Console.Write("Enter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

          
                Customer customer = customerRepo.GetCustomerById(customerId);
                if (customer == null)
                {
                    throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                }

                
                Console.Write("Enter Product ID: ");
                int productId = int.Parse(Console.ReadLine());

                
                Product product = productRepo.GetProductById(productId);
                if (product == null)
                {
                    throw new ProductNotFoundException($"Product with ID {productId} not found.");
                }

                
                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                if (quantity <= 0 || quantity > product.StockQuantity)
                {
                    Console.WriteLine($"Invalid quantity. Please enter a quantity between 1 and {product.StockQuantity}.");
                    return;
                }

              
                bool result = cartRepo.AddToCart(customerId, productId, quantity);

                if (result)
                {
                    Console.WriteLine("\nProduct added to cart successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to add product to cart.");
                }
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

          
           
            Console.ReadKey();
        }


private static void ViewCart()
{
    try
    {
        Console.Clear();
        Console.WriteLine("**View Cart**");

  
        Console.Write("Enter Customer ID: ");
        int customerId = int.Parse(Console.ReadLine());

   
        List<Cart> cartItems = cartRepo.GetCartItemsByCustomerId(customerId);

        if (cartItems != null && cartItems.Count > 0)
        {
            Console.WriteLine("\nYour Cart Items:");
            foreach (var cartItem in cartItems)
            {
               
                if (cartItem.Price > 0)
                {
                    cartItem.TotalPrice = cartItem.Quantity * cartItem.Price; 
                }
                else
                {
                    cartItem.TotalPrice = 0;
                }

               
                Console.WriteLine($"Product Name: {cartItem.ProductName}, " +
                                  $"Product ID: {cartItem.ProductId}, " +
                                  $"Quantity: {cartItem.Quantity}, " +
                                  $"Price: {cartItem.Price:C}, " + 
                                  $"Total Price: {cartItem.TotalPrice:C}"); 
            }
        }
        else
        {
            Console.WriteLine("\nYour cart is empty.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    }


    Console.WriteLine("\nPress any key ");
    Console.ReadKey();
}
        private static void PlaceOrder()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("**Place Order **");

                Console.Write("Enter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

               
                Customer customer = customerRepo.GetCustomerById(customerId);
                if (customer == null)
                {
                    throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                }

                Console.Write("Enter Shipping Address: ");
                string shippingAddress = Console.ReadLine();

              
                List<Cart> cartItems = cartRepo.GetCartItemsByCustomerId(customerId);

                if (cartItems == null || cartItems.Count == 0)
                {
                    Console.WriteLine("Your cart is empty. Cannot place order.");
                    return;
                }

              
                bool orderPlaced = orderRepo.PlaceOrder(customerId, cartItems, shippingAddress);

                if (orderPlaced)
                {
                    Console.WriteLine("Order placed successfully!");
                    cartRepo.ClearCart(customerId); 
                }
                else
                {
                    Console.WriteLine("Failed to place order.");
                }
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key ");
            Console.ReadKey();
        }


        private static void ViewCustomerOrders()
        {
            try
            {
               
                Console.Clear();
                Console.WriteLine("**View Customer Orders**");
                Console.Write("Enter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

              
                List<Order> orders = orderRepo.GetCustomerOrders(customerId);

                if (orders != null && orders.Count > 0)
                {
                    Console.WriteLine($"\nOrders for Customer ID: {customerId}");

                    foreach (var order in orders)
                    {
                        Console.WriteLine($"Order ID: {order.OrderId}, Date: {order.OrderDate.ToShortDateString()}, " +
                                          $"Total Price: {order.TotalPrice:C}, Shipping Address: {order.ShippingAddress}");

                  
                        List<OrderItem> orderItems = orderRepo.GetOrderItemsByOrderId(order.OrderId);
                        if (orderItems != null && orderItems.Count > 0)
                        {
                            Console.WriteLine("Order Items:");
                            foreach (var item in orderItems)
                            {
                                Console.WriteLine($"- Product ID: {item.ProductId}, Quantity: {item.Quantity}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No items in this order.");
                        }

                        Console.WriteLine("------------");
                    }
                }
                else
                {
                    Console.WriteLine("No orders found for this customer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

          
            Console.WriteLine("\nPress any key ..");
            Console.ReadKey();
        }
    }
}
