using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    /// <summary>
    /// Represents a Service Layer exception.
    /// </summary>
    public class SLException : Exception
    {
        public ErrorDetails ErrorDetails { get; set; }

        internal SLException(string message, ErrorDetails errorDetails, Exception innerException) : base(message, innerException)
        {
            ErrorDetails = errorDetails;
        }
        internal SLException(string message, Exception innerException) : base(message, innerException)
        {
     
        }
    }
    public class ErrorDetails
    {

        public int Code { get; set; }

        public ErrorMessage Message { get; set; }
    }
    public class ErrorMessage
    {

        public string Lang { get; set; }

        public string Value { get; set; }
    }
}
