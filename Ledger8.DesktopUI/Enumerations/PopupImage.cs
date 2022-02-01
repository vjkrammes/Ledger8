using System.ComponentModel;

namespace Ledger8.DesktopUI.Enumerations;

public enum PopupImage
{
    [Description("None")]
    None = 0,
    [Description("Question")]
    Question = 1,
    [Description("Information")]
    Information = 2,
    [Description("Warning")]
    Warning = 3,
    [Description("Stop")]
    Stop = 4,
    [Description("Error")]
    Error = 5
}
