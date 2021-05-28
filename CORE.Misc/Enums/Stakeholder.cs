using System.ComponentModel;

namespace CORE.Misc.Enums
{
    public enum Stakeholder
    {
        [Description("Brunei National Petroleum")]
        BruneiNationalPetroleum,
        [Description("Integrated Technical Review")]
        IntegratedTechnicalReview,
        [Description("Committee")]
        Committee,
        [Description("Internal Commercial Task")]
        InternalCommercialTask,
        NotAvailable = -1
    }
}