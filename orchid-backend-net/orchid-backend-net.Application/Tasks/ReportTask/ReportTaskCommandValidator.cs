using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tasks.ReportTask
{
    public class ReportTaskCommandValidator : AbstractValidator<ReportTaskCommand>
    {
        public ReportTaskCommandValidator() 
        {
            Configuration();
        }
        private void Configuration()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .NotNull()
                .WithMessage("File name cannot be empty.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .NotEmpty()
                .WithMessage("File cannot be empty");
        }
    }
}
