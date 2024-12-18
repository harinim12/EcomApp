using System;

namespace EcommerceApplication.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        // Default constructor
        public OrderNotFoundException()
            : base("Order not found.")
        { }

        // Constructor that accepts a custom message
        public OrderNotFoundException(string message)
            : base(message)
        { }

        // Constructor that accepts a custom message and an inner exception
        public OrderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
