using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsUserClaims")]
    public class RIMSUserClaims : IdentityUserClaim<string>
    {
        [Column("Id")]
        public override int Id { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual RIMSUsers? User { get; set; } // Made nullable
    }
}