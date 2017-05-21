using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Domain.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<AllowanceTask> AllowanceTasks { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }

        public decimal Balance()
        {
            if (Entries == null)
            {
                return 0m;
            }
            return Entries.Where(e => e.UserId == Id).Sum(e => e.Price);
        }

        public decimal SpendingForMonth(DateTime date)
        {
            if (Entries == null)
            {
                return 0m;
            }
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            return Entries.Where(e => e.UserId == Id)
                .Where(e => e.EntryDate >= firstDay && e.EntryDate <= lastDay)
                .Where(e => e.Price > 0)
                .Sum(e => e.Price);
        }
    }
}
