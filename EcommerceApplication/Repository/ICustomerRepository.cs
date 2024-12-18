using EcommerceApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Repository
{
    public interface ICustomerRepository
    {
        
        bool RegisterCustomer(Customer customer); 
        Customer GetCustomerById(int customerId);
    }
}

