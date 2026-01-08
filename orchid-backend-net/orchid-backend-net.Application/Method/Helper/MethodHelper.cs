using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Helper
{
    public static class MethodHelper
    {
        public static void AddMethodWithResourceHelper(Methods method, List<CreateMethodDto> methodDto)
        {
            if (methodDto.Count == 0)
                throw new InvalidOperationException("Không thể tạo method mà không có stage nào.");
            methodDto.ForEach(x =>
            {
                 method.AddMethodStageWithResource(
                     x.StageDefinitionId,
                     x.Order,
                     x.DurationDays,
                     x.CreateMaterial,
                     x.CreateChemical,
                     x.CreateSampleRequirement);
            });
        }
    }
}
