using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Enums
{
    public enum UserType
    {
        // کاربر عادی یا میهمان
        User = 1,

        // کابر عضو باشگاه
        SpecialUser = 2,

        // ادمین وب سایت
        AdminUser = 3,

        // مدیر سایت
        Manager = 4
    }
}
