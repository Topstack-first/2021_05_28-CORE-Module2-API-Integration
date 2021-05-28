using System.ComponentModel;

namespace CORE.Misc.Enums
{
    public enum Category
    {
        [Description("Agreement")]
        Agreement,
        [Description("Audit")]
        Audit,
        [Description("Compliance")]
        Compliance,
        [Description("Design")]
        Design,
        [Description("Email And Letter")]
        EmailAndLetter,
        NotAvailable = -1
    }
}