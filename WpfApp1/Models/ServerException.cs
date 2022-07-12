using System;

namespace WpfApp1.Models
{
    internal class ServerException : Exception
    {
        public ServerException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
