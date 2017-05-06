using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Budget.Domain.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<AllowanceTask> AllowanceTasks { get; set; }
        public ICollection<Entry> Entries { get; set; }
    }
}
