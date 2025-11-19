using FluentValidation;

namespace orchid_backend_net.Application.TissueCultureBatch.CreateTissueCultureBatch
{
    public class CreateTissueCultureBatchCommandValidator : AbstractValidator<CreateTissueCultureBatchCommand>
    {
        public CreateTissueCultureBatchCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {

        }
    }
}
