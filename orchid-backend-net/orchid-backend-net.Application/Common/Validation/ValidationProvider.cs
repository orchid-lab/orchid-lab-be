using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace orchid_backend_net.Application.Common.Validation
{
    public class ValidationProvider : IValidationProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IValidator<T> GetValidator<T>()
        {
            return _serviceProvider.GetService<IValidator<T>>()!;
        }
    }
}
