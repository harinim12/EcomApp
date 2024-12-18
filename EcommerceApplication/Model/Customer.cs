using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Model
{
    public class Customer
    {
        private int _customerId;
        private string _name;
        private string _email;
        private string _password;

      
        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

    
        public Customer() { }

        public Customer(int customerId, string name, string email, string password)
        {
            _customerId = customerId;
            _name = name;
            _email = email;
            _password = password;
        }

       
        public override string ToString()
        {
            return $"CustomerId: {_customerId}, Name: {_name}, Email: {_email}";
        }
    }
}

