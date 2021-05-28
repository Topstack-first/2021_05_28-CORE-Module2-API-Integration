using System.ComponentModel;

namespace CORE.Misc.Enums
{
    public enum Event
    {
        [Description("Assurance Review")]
        AssuranceReview,
        [Description("Contractor Risk Opportunity")]
        ContractorRiskOpportunity,
        [Description("Framing Workshop")]
        FramingWorkshop,
        NotAvailable = -1
    }
}