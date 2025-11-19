using FluentValidation;

namespace orchid_backend_net.Application.TissueCultureBatch.DeleteTissueCultureBatch
{
    public class DeleteTissueCultureBatchCommandValidator : AbstractValidator<DeleteTissueCultureBatchCommand>
    {
        public DeleteTissueCultureBatchCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {

        }
    }
}
