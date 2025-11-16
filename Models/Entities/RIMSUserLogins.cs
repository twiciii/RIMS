using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsUserLogins")]
    public class RIMSUserLogins : IdentityUserLogin<string>
    {
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual RIMSUsers? User { get; set; } // Made nullable
    }
}