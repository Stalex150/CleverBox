using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBox.Models.ViewModels
{
    public class HomeViewModel
    {
        public int UserPoints { get; set; }
        public int FactInMemory { get; set; }
        public int LearnedFacts { get; set; }
        public int AllFacts { get; set; }
    }
}
