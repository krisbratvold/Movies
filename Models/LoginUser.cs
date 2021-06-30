using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "is required")]
        [EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "is required")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword { get; set; }
    }
}