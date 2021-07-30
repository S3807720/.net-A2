using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Web.ViewModels
{
    public class LoginViewModel
    {

        [StringLength(8)]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}