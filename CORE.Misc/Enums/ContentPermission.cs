using System;
using System.ComponentModel;

namespace CORE.Misc.Enums
{
    [Flags]
    public enum ContentPermission
    {
        AllDepartments = 0,
        Private = 1,
        Administrators = 2,
        Finance = 4
    }
}