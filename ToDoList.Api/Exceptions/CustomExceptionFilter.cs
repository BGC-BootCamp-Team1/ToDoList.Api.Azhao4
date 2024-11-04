using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CustomExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var response = new
        {
            Message = "An error occurred while processing your request.",
            Detail = context.Exception.Message
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;

        await Task.CompletedTask;
    }
}