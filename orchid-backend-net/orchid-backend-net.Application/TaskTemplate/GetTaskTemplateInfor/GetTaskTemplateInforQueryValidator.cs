using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.GetTaskTemplateInfor
{
    public class GetTaskTemplateInforQueryValidator : AbstractValidator<GetTaskTemplateInforQuery>
    {
        public GetTaskTemplateInforQueryValidator() 
        {
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Missing task template ID");
        }
    }
}
