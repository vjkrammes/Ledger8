using System.ComponentModel;

namespace Ledger8.Common.Enumerations;

public enum DueDateType
{
    [Description("Unspecified")]
    Unspecified = 0,
    [Description("Monthly")]
    Monthly = 1,
    [Description("Quarterly")]
    Quarterly = 2,
    [Description("Annually")]
    Annnually = 3,
    [Description("Service Related")]
    ServiceRelated = 4,
    [Description("Not Applicable")]
    NA = 5,
    [Description("Semi-annual")]
    SemiAnnual = 6
}
