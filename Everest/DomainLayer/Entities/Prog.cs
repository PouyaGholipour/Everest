using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Prog
    {
        public Prog()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? LeaderName { get; set; }
        public string Place { get; set; }
        public string? WhichProgramPrerequisites { get; set; }
        public string? PrerequisitePrograms { get; set; }
        public string? PrerequisiteCourse { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime DateOfHolding { get; set; }
        public ProgramType ProgramType { get; set; }

        #region Relations for navigation Property

        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ProgUser>? ProgUsers { get; set; }

        #endregion
    }
}
