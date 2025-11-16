using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsUserRoles")]
    public class RIMSUserRoles : IdentityUserRole<string>
    {
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual RIMSUsers? User { get; set; } // Made nullable

        [ForeignKey("RoleId")]
        public virtual RIMSRoles? Role { get; set; } // Made nullable
    }
}