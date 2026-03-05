using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MecGestor.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Exception exceptionToHandle;

        if (exception is ValidationException validationException)
        {
            _logger.LogWarning("Erro de validação: {Errors}",
                string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage)));

            exceptionToHandle = exception;
        }
        else
        {
            exceptionToHandle = exception is DbUpdateException dbEx && dbEx.InnerException != null
                ? dbEx.InnerException
                : exception;

            _logger.LogError(exception, "Erro não tratado: {Message}", exceptionToHandle.Message);
        }

        await HandleExceptionAsync(context, exceptionToHandle, cancellationToken);

        return true;
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, errors, detail) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "Erro de validação",
                validationEx.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList(),
                (string?)null
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Acesso não autorizado",
                new List<string>(),
                (string?)null
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Recurso não encontrado",
                new List<string>(),
                exception.Message
            ),
            ArgumentException => (
                HttpStatusCode.BadRequest,
                "Dados inválidos",
                new List<string>(),
                exception.Message
            ),
            InvalidOperationException => (
                HttpStatusCode.BadRequest,
                "Operação inválida",
                new List<string>(),
                exception.Message
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Erro interno do servidor",
                new List<string>(),
                _environment.IsDevelopment() ? exception.Message : null
            )
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (errors.Count > 0)
            problemDetails.Extensions["errors"] = errors;

        if (_environment.IsDevelopment() && exception is not ValidationException)
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}