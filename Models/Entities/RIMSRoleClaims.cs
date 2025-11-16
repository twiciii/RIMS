using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsRoleClaims")]
    public class RIMSRoleClaims : IdentityRoleClaim<string>
    {
        [Column("Id")]
        public override int Id { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual RIMSRoles Role { get; set; } = null!;
    }
}