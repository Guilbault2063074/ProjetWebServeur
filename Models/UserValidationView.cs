using System.ComponentModel.DataAnnotations;

namespace Projet_Web_Serveur.Models
{
    public class UserValidationView
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter an email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
