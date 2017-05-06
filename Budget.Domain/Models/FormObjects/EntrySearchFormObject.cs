using System;

namespace Budget.Domain.Models.FormObjects
{
    /// <summary>
    /// Prepare searchable fields for Entry records
    /// </summary>
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
