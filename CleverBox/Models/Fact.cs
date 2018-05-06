using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBox.Models
{
    public class Fact
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Display(Name = "Memory level")]
        public int RepetitionLevel { get; set; }

        [Display(Name = "Next time")]
        public DateTime NextTime { get; set; }
    }
}
