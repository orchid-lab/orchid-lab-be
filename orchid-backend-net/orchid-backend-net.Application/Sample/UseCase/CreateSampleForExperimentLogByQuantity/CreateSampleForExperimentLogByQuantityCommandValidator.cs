using FluentValidation;
using orchid_backend_net.Application.Sample.UseCase.CreateSampleByQuantity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Sample.UseCase.CreateSampleForExperimentLogByQuantity
{
    public class CreateSampleForExperimentLogByQuantityCommandValidator : AbstractValidator<CreateSampleForExperimentLogByQuantityCommand>
    {
        public CreateSampleForExperimentLogByQuantityCommandValidator()
        {
            RuleFor(x => x.ExperimentLogId)
                .NotEmpty().WithMessage("ExperimentLogId không được để trống.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity phải lớn hơn 0.");
        }
    }
}
