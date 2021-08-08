

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Admin.Models
{
    public class Login
    {
        [Column(TypeName = "nchar")]
        [StringLength(8)]
        public string LoginID { get; set; }
        public bool CanLogin { get; set; } = true;

        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string Password { get; set; }

    }
}
