using EcommerceApplication.Model;

namespace EcommerceApplication.Repository
{
    public interface IProductRepository
    {
        bool CreateProduct(Product product);
        
        bool DeleteProduct(int productId);
        Product GetProductById(int productId);
        bool PlaceOrder(Customer customer, List<Cart> cartItems, string shippingAddress);
        
    }

}
