using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Infrastructure
{
    public enum StateEnum
    {
        Start,
        End,
        InProgress,
        Approved,
        Refused,
        WaitApproving,
        Allocated,
        Withdraw
    }
}