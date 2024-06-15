﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        #region Relations for navigation property

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Prog> Progs { get; set; }

        #endregion
    }
}
