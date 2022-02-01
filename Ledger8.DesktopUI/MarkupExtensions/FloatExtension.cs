using System;
using System.Windows.Markup;

namespace Ledger8.DesktopUI.MarkupExtensions;

public sealed class FloatExtension : MarkupExtension
{
    private float _value { get; }
    public FloatExtension(float value) => _value = value;
    public override object ProvideValue(IServiceProvider _) => _value;
}
