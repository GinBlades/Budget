using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Models.FormObjects
{
    public class EntrySearchFormObject
    {
        public string User { get; set; }
        public string Payee { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public int? Page { get; set; }
        public int PerPage { get; set; } = 20;
    }
}
