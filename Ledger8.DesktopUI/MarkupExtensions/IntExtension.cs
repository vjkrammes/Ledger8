using System;
using System.Windows.Markup;

namespace Ledger8.DesktopUI.MarkupExtensions;

public sealed class IntExtension : MarkupExtension
{
    private int _value { get; }
    public IntExtension(int value) => _value = value;
    public override object ProvideValue(IServiceProvider _) => _value;
}
