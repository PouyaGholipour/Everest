using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }

        #region Relations for navigation property

        public ICollection<RoleUser> RoleUsers { get; set; }

        #endregion
    }
}
