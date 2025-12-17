using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace HiloGame.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await _next(context);
        }
		catch (Exception ex)
		{
            await HandleExceptionAsync(context, ex);

        }
        
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = StatusCodes.Status500InternalServerError;
        var result = string.Empty;
        
        if(exception is ValidationException validationException)
        {
            code = StatusCodes.Status400BadRequest;

            result = JsonSerializer.Serialize(new 
            { 
                errors = validationException.Errors.Select(e => e.ErrorMessage) 
            });
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result ?? JsonSerializer.Serialize(new { error = exception.Message }));
    }
}
