using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Results
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; } 

        public static Result<T> SuccessResult(T data, string? message = null)
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }


    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    
        public static Result SuccessResult(string? message = null)
        {
            return new Result
            {
                Success = true,
                Message = message,
              
            };
        }

        public static Result Fail(string? message)
        {
            return new Result
            {
                Success = false,
                Message = message,
               
            };
        }
    }
}
