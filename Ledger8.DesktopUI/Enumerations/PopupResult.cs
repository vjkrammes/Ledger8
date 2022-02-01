using System.ComponentModel;

namespace Ledger8.DesktopUI.Enumerations;

public enum PopupResult
{
    [Description("Unspecified")]
    Unspecified = 0,
    [Description("Yes")]
    Yes = 1,
    [Description("No")]
    No = 2,
    [Description("Ok")]
    Ok = 3,
    [Description("Cancel")]
    Cancel = 4
}
