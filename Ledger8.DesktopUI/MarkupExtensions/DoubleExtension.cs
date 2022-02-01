using System;
using System.Windows.Markup;

namespace Ledger8.DesktopUI.MarkupExtensions;

public sealed class DoubleExtension : MarkupExtension
{
    private double _value { get; }
    public DoubleExtension(double value) => _value = value;
    public override object ProvideValue(IServiceProvider _) => _value;
}
