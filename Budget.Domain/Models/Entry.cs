using System;
using System.ComponentModel.DataAnnotations;

namespace Budget.Domain.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string Payee { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime EntryDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
