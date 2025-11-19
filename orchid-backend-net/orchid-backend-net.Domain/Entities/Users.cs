using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Users : BaseSoftDelete
    {
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public int RoleID { get; set; }
        [ForeignKey(nameof(RoleID))]
        public virtual Role Role { get; set; }
        public string? AvatarUrl { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<TasksAssign> Assigns { get; set; }
    }
}
