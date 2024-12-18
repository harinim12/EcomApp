using NUnit.Framework;
using EcommerceApplication.Repository;
using EcommerceApplication.Model;
using EcommerceApplication.Exceptions;

namespace EcommerceApplication.Test
{
    public class EcommerceTest
    {
        [Test]
        public void createProductTest()
        {
            var product = new Product
            {
                Name = "Test Product",
                Price = 99.99m,
                Description = "This is a test product.",
                StockQuantity = 10
            };
            var productService = new ProductRepository(); 

          
            bool result = productService.CreateProduct(product);

        
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public void AddToCartTest()
        {
            var customerId = 1; 
            var productId = 1;  
            var quantity = 2;   

            var cartRepository = new CartRepository();
            bool result = cartRepository.AddToCart(customerId, productId, quantity);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public void PlaceOrderTest()
        {
            int customerId = 1; 
            string shippingAddress = "123 Test Street";
            var cartItems = new List<Cart>
            {
                new Cart { ProductId = 1, Quantity = 2 }, 
                new Cart { ProductId = 2, Quantity = 1 }
            };
            var orderRepository = new OrderRepository();
            bool result = orderRepository.PlaceOrder(customerId, cartItems, shippingAddress);

          
            Assert.That(result, Is.EqualTo(true));
        }

       
        





    }
}
