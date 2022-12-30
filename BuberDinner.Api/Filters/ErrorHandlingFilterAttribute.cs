using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters;

public class ErrorHandlingFilterAttribute:ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
              var exception =context.Exception;
              
              
              var problemDetails=new ProblemDetails{
                    Type="https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title="An Error occurred while processing your request",
                    Status=(int)HttpStatusCode.InternalServerError
              };

              //var errorResult = new { error = "An error occurred while processing your request" };
              context.Result=new ObjectResult(problemDetails)
               {
                 StatusCode=500
               };
               context.ExceptionHandled=true;
    }
    
}