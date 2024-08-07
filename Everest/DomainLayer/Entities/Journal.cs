using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Journal
    {
        public int Id { get; set; }
        public string JournalTitle { get; set; }
        public string Content { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime CreateDate { get; set; }
    }
}
