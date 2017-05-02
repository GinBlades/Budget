using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Models
{
    public class AllowanceTask
    {
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
