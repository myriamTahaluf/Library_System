using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enum
{
    public enum AuthenticateResultEnum
    {
        wrongCredinal = 0,  
        Ok=1,
        AccountDisabled = 2,
        Authenticated = 3,
        ValidationError = 4,
    }
}
