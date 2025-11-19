using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Report.GetReportInfor
{
    public class GetReportInforQueryValidator : AbstractValidator<GetReportInforQuery>
    {
        public GetReportInforQueryValidator() 
        {
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
        }
    }
}
