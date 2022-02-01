using System.ComponentModel;

namespace Ledger8.Common.Enumerations;

public enum PasswordStrength
{
    [Description("Blank")]
    Blank = 0,
    [Description("Very weak")]
    VeryWeak = 1,
    [Description("Weak")]
    Weak = 2,
    [Description("Medium")]
    Medium = 3,
    [Description("Strong")]
    Strong = 4,
    [Description("Very strong")]
    VeryStrong = 5
}
