using System;
using System.ComponentModel;

namespace Ledger8.DesktopUI.Enumerations;

[Flags]
public enum PopupButtons
{
    [Description("None")]
    None = 0,
    [Description("Ok")]
    Ok = 1,
    [Description("Cancel")]
    Cancel = 2,
    [Description("Yes")]
    Yes = 4,
    [Description("No")]
    No = 8,
    [Description("Ok and Cancel")]
    OkCancel = Ok | Cancel,
    [Description("Yes and No")]
    YesNo = Yes | No,
    [Description("Yes, No and Cancel")]
    YesNoCancel = Yes | No | Cancel
}
