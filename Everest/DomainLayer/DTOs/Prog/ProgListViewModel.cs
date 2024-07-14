using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Prog
{
    public class ProgListViewModel
    {
        public List<Entities.Prog> Progs { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string? ProgTitleFilter { get; set; }
        public int PageId { get; set; }
    }
}
