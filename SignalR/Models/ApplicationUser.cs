using Microsoft.AspNetCore.Identity;

namespace SignalR.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public bool isDeleted { get; set; }
    }
}
