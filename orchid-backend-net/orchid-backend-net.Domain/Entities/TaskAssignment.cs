using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskAssignment : BaseGuidEntity
    {
        public string TaskId { get; set; }
        public required string TechnicianId { get; set; } 
        public string? SampleId { get; set; } 
        public bool IsForWholeExperimentLog { get; set; }   

        [ForeignKey(nameof(TechnicianId))]
        public virtual Users Technician { get; set; } 
        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Task { get; set; } 
        [ForeignKey(nameof(SampleId))]
        public virtual Samples Sample { get; set; }
        /// <summary>
        /// the created date of the task
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// the actual end date of the task
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// the expected end date of the task
        /// </summary>
        public DateTime ExpectedEndDate { get; set; }
    }
}
