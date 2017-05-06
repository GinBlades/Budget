using Budget.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Web.Models.FormObjects
{
    public class EntrySearchFormObject
    {
        public string User { get; set; }
        public string Payee { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var prop in GetType().GetProperties())
            {
                if (prop.GetValue(this, null) != null)
                {
                    dictionary[prop.Name] = prop.GetValue(this, null);
                }
            }
            return dictionary;
        }
    }
}
