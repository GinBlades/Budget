using Budget.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Budget.Domain.Models
{
    public class AllowanceTask : IDBModelTS
    {
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reward { get; set; }

        // TODO: This is more trouble than I thought. Switch to JSON?
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

        public List<string> ToDaysCompleted()
        {
            var days = new List<string>();
            var binFormat = Convert.ToString(Days, 2);
            var intFormat = Int32.Parse(binFormat);
            var paddedFormat = intFormat.ToString("D7");
            for (var i = 0; i < paddedFormat.Length; i++)
            {
                if (paddedFormat[i] == '1')
                {
                    days.Add(WeekDays[i]);
                }
            }
            return days;
        }

        public string[] WeekDays => new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public int FromDaysCompleted(string[] days)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < WeekDays.Length; i++)
            {
                if (days.Contains(WeekDays[i]))
                {
                    sb.Append("1");
                } else
                {
                    sb.Append("0");
                }
            }

            return Convert.ToInt32(sb.ToString(), 2);
        }

        public void AddDays(string day)
        {
            var daysCompleted = ToDaysCompleted();
            var index = Array.IndexOf(WeekDays, day);
            if (index >= 0)
            {
                daysCompleted.Add(WeekDays[index]);
            }
            Days = FromDaysCompleted(daysCompleted.ToArray());
        }
    }
}
