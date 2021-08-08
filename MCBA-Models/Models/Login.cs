

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Models.Models
{
    public class Login
    {
        [Column(TypeName = "nchar")]
        [StringLength(8)]
        public int LoginID { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

    }
}
