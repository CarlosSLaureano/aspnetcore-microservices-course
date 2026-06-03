using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace GeekShopping.Web.Utils
{
    /// <summary>
    /// Model binder que aceita tanto vírgula (pt-BR: 78,90) quanto
    /// ponto (en-US: 78.90) como separador decimal.
    /// </summary>
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(
                bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(value))
                return Task.CompletedTask;

            // Normaliza: substitui vírgula por ponto para parsing invariante
            var normalized = value.Replace(",", ".");

            // Se houver mais de um ponto (ex: "1.234.56"), remove os separadores de milhar
            var dotCount = normalized.Count(c => c == '.');
            if (dotCount > 1)
            {
                // Último ponto é o decimal — remove os anteriores
                var lastDot = normalized.LastIndexOf('.');
                normalized = normalized.Remove(0, lastDot - normalized.Length + 1)
                    .Replace(".", "") + "." + normalized.Substring(lastDot + 1);
                // Re-normaliza de forma mais simples:
                normalized = value.Replace(".", "").Replace(",", ".");
            }

            if (decimal.TryParse(normalized, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    $"O valor '{value}' não é um número decimal válido.");
            }

            return Task.CompletedTask;
        }
    }

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(decimal) ||
                context.Metadata.ModelType == typeof(decimal?))
            {
                return new DecimalModelBinder();
            }
            return null;
        }
    }
}
