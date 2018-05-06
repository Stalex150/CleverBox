using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBox.Models.ViewModels
{
    public class TestViewModel
    {
        public Fact Fact { get; set; }

        [Required]
        public string InputText { get; set; }
    }
}
