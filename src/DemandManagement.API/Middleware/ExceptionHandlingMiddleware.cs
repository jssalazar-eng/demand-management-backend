using System.Net;
using System.Text.Json;
using DemandManagement.Domain.Exceptions;
using FluentValidation;

namespace DemandManagement.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        HttpStatusCode statusCode;
        string message;
        object? errors;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                message = "Validation failed";
                errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                break;

            case NotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundEx.Message;
                errors = null;
                break;

            case DomainException domainEx:
                statusCode = HttpStatusCode.BadRequest;
                message = domainEx.Message;
                errors = null;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An internal server error occurred";
                errors = null;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}