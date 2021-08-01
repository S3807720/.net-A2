

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.Models
{
    public class Login
    {
        [Column(TypeName = "nchar")]
        [StringLength(8)]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

    }
}
