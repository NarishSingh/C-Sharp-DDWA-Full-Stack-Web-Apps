using System.ComponentModel.DataAnnotations;

namespace MovieCatalogIdentity.Models
{
    public class LoginVM
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}