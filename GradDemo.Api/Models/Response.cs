using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradDemo.Api.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }

        public static Response<T> Error(string message)
            => new Response<T> { Message = message };

        public static Response<T> Successful(T payload)
            => new Response<T> { Success = true, Payload = payload };

        public override string ToString()
        {
            if (Success)
                return $"Successful: {Payload?.ToString()}";

            return $"Error: {Message}";
        }
    }
}
