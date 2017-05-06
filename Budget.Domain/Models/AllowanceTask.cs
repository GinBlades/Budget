using Budget.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Budget.Domain.Models
{
    public class AllowanceTask : IDBModelTS
    {
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reward { get; set; }

        // Using binary to set days.
        // Sunday = 0b1, Saturday = 0b100_0000
        // To get binary from int: 'Convert.ToString(105, 2)' = '1101001'
        // To get int from binary string: 'Convert.ToInt32("1101", 2)'
        public int Days { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
