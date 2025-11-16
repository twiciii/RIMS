using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsUsers")]
    public class RIMSUsers : IdentityUser
    {
        [Column("UserId")]
        public override string Id { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RIMSAuditTrail> AuditTrails { get; set; } = new List<RIMSAuditTrail>();
        public virtual ICollection<RIMSUserClaims> Claims { get; set; } = new List<RIMSUserClaims>();
        public virtual ICollection<RIMSUserLogins> Logins { get; set; } = new List<RIMSUserLogins>();
        public virtual ICollection<RIMSUserRoles> UserRoles { get; set; } = new List<RIMSUserRoles>();
    }
}