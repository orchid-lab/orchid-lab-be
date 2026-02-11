using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep;

namespace orchid_backend_net.API.Dto.SafeProcedure
{
    /// <summary>
    /// for updating safe procedure information
    /// </summary>
    public class UpdateSafeProcedureDto
    {
        /// <summary>
        /// name of safe procedure
        /// </summary>
        public string? ProcedureName { get; set; }
        /// <summary>
        /// description of safe procedure
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// type of safe procedure
        /// </summary>
        public string? ProcedureType { get; set; }
        /// <summary>
        /// step of safe procedure
        /// </summary>
        public List<UpdateSafeProcedureStepDto>? SafeProcedureSteps { get; set; }
    }
}
