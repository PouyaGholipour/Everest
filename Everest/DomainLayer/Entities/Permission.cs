using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }

        #region Relations for navigation property

        [ForeignKey(nameof(ParentId))]
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
