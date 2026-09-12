using System.Net;
using System.Text.Json;
using FluentValidation;
using HospitalProMedico.Application.Common;

namespace HospitalProMedico.API.Middleware;

public class ExceptionHandlingMiddleware
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
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "Um ou mais erros de validação ocorreram.",
                validationEx.Errors.Select(e => e.ErrorMessage).ToArray()
            ),
            NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message, Array.Empty<string>()),
            BusinessRuleException businessEx => (HttpStatusCode.BadRequest, businessEx.Message, Array.Empty<string>()),
            UnauthorizedAppException unauthorizedEx => (HttpStatusCode.Unauthorized, unauthorizedEx.Message, Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado. Tente novamente mais tarde.", Array.Empty<string>())
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Erro de aplicação: {Message}", exception.Message);
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message,
            errors,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
