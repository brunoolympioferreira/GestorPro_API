using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestorPro.Api.Filters;

public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var argumentType = argument.GetType();

            // Tenta obter o validator para este tipo
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                // Cria o contexto de validação
                var validationContext = new ValidationContext<object>(argument);

                // Executa a validação
                var validationResult = await validator.ValidateAsync(validationContext);

                // Se houver erros, lança ValidationException que será capturada pelo middleware
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }
        }

        await next();
    }
}
