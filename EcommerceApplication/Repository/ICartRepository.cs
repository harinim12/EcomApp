using EcommerceApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Repository
{
    public interface ICartRepository
    {
       
        bool AddToCart(int customerId, int productId, int quantity);
        List<Cart> GetCartItemsByCustomerId(int customerId);
        void ClearCart(int customerId);
    }
}
