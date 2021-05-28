using System.ComponentModel;

namespace CORE.Misc.Enums
{
    public enum Location
    {
        [Description("Brunei")]
        Brunei,
        [Description("Convention Centre, KL")]
        ConventionCentreKL,
        NotAvailable = -1
    }
}