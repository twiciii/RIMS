using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsRoles")]
    public class RIMSRoles : IdentityRole<string>
    {
        [Column("Id")]
        public override string Id { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RIMSRoleClaims> RoleClaims { get; set; } = new List<RIMSRoleClaims>();
        public virtual ICollection<RIMSUserRoles> UserRoles { get; set; } = new List<RIMSUserRoles>();
    }
}