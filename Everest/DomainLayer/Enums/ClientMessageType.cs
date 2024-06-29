using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Enums
{
    public enum ClientMessageType
    {
        Default = 0,
        Null = 1,
        Success = 2,
        Failed = 3,
        IsNotActive = 4,
        EmailIsExist = 5,
        UsernameIsExist = 6,
        UsernameEmaliIsExist = 7,
    }
}
