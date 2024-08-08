using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Role
{
    public class CreateRoleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
