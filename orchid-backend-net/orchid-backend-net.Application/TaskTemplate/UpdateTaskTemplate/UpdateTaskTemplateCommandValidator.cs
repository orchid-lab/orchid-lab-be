using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.UpdateTaskTemplate
{
    public class UpdateTaskTemplateCommandValidator : AbstractValidator<UpdateTaskTemplateCommand>
    {
        public UpdateTaskTemplateCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {

        }
    }
}
