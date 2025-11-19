using FluentValidation;

namespace orchid_backend_net.Application.Common.Validation
{
    public interface IValidationProvider
    {
        IValidator<T> GetValidator<T>();
    }
}
