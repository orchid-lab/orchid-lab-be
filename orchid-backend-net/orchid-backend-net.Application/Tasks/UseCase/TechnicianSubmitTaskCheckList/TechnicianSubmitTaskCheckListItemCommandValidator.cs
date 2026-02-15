using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.TechnicianSubmitTaskCheckList
{
    public class TechnicianSubmitTaskCheckListItemCommandValidator : AbstractValidator<TechnicianSubmitTaskCheckListItemCommand>
    {
        public TechnicianSubmitTaskCheckListItemCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId không được để trống");
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId không được để trống");
            RuleFor(x => x.MeasurementUnit).NotEmpty().WithMessage("MeasurementUnit không được để trống");
            RuleFor(x => x.MeasuredValue).GreaterThanOrEqualTo(0).WithMessage("MeasuredValue phải lớn hơn hoặc bằng 0");
        }
    }
}
