using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Prog
{
    public class HeldProgListViewModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Place { get; set; }
        public DateTime? DateOfHolding { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
    }
}
