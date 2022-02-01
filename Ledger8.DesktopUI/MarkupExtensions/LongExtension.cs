using System;
using System.Windows.Markup;

namespace Ledger8.DesktopUI.MarkupExtensions;

public sealed class LongExtension : MarkupExtension
{
    private long _value { get; }
    public LongExtension(long value) => _value = value;
    public override object ProvideValue(IServiceProvider _) => _value;
}
