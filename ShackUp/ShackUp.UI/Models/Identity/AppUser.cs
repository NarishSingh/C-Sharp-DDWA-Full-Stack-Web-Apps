using Microsoft.AspNet.Identity.EntityFramework;

namespace ShackUp.UI.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string StateId { get; set; }
    }
}