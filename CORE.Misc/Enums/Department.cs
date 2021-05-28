using System.ComponentModel;

namespace CORE.Misc.Enums
{
    public enum Department
    {
        [Description("Adminstration")]
        Adminstration,
        [Description("Business Planning")]
        BusinessPlanning,
        [Description("Commercial")]
        Commercial,
        [Description("Exploration")]
        Exploration,
        [Description("Finance")]
        Finance,
        NotAvailable = -1
    }
}