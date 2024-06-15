using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Enums
{
    public enum CommentStatus
    {
        // منتظر تایید
        WaitConfirmation = 1,

        // تایید
        Confirmation = 2,

        // عدم تایید
        disapproval = 3
    }
}
