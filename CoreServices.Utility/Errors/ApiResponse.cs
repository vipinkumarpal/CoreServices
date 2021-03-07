using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Utility.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public Fault Fault { get; set; }
        public object Result { get; set; }
        public ApiResponse(int statusCode, string message ="", object result = null, Fault fault = null)
        {
            StatusCode = statusCode;
            Type = message;
            Result = result;
            Fault = fault;
        }
    }
}
