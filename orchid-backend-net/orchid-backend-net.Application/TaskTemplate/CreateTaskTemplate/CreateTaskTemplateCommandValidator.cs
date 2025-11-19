using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.CreateTaskTemplate
{
    public class CreateTaskTemplateCommandValidator : AbstractValidator<CreateTaskTemplateCommand>
    {
        public CreateTaskTemplateCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {

        }
    }
}
