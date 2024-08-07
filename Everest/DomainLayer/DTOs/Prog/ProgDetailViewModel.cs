using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Prog
{
    public class ProgDetailViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? HoldingDate { get; set; }
        public string? LeaderName { get; set; }
        public string? WhichProgramPrerequisites { get; set; }
        public string? PrerequisitePrograms { get; set; }
        public string? PrerequisiteCourse { get; set; }
        public string? Image { get; set; }
        public string? Place { get; set; }
        public decimal? Price { get; set; }
        public string? ProgramType { get; set; }
    }
}
