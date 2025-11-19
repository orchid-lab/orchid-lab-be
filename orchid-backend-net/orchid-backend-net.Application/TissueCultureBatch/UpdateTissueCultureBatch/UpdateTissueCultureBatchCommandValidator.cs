using FluentValidation;

namespace orchid_backend_net.Application.TissueCultureBatch.UpdateTissueCultureBatch
{
    public class UpdateTissueCultureBatchCommandValidator : AbstractValidator<UpdateTissueCultureBatchCommand>
    {
        public UpdateTissueCultureBatchCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {

        }
    }
}
