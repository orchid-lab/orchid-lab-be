using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep
{
    public record UpdateSafeProcedureStepDto(string Id, string? SafeProcedureStepName, string? SafeProcedureType, string? Description);
}
