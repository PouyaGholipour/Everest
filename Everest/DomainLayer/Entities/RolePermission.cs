using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class RolePermission
    {
        public int RP_Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        #region Relations for navigation property

        public Role Role { get; set; }
        public Permission Permission { get; set; }

        #endregion
    }
}
