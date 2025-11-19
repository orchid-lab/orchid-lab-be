using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Disease.Analysis
{
    public class DiseaseAnalysisCommandValidator : AbstractValidator<DiseaseAnalysisCommand>
    {
        public DiseaseAnalysisCommandValidator()
        {
            RuleFor(command => command.ImageBytes)
                .NotNull()
                .WithMessage("Image cannot be null.")
                .NotEmpty()
                .WithMessage("Image cannot be empty.");
        }
    }
}
