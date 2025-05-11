using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string?message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    
     private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "a bad request, you have made",
            401 => "Authorized, you are not. get a token first",
            404 => "Resource found, it was not",
            500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change.",
            _ => "An unexpected error has occurred"
        };
    }

}