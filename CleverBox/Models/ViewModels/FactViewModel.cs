using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBox.Models.ViewModels
{
    public class FactViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Display(Name = "Level")]
        public string RepetitionLevel { get; set; }

        [Display(Name = "Next time")]
        public string NextTime { get; set; }
    }
}
