using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ledger8.DesktopUI.Controls;

public class ClippingBorder : Border
{
    private readonly RectangleGeometry _clipRectangle = new();
    private object? _oldClip;

    protected override void OnRender(DrawingContext dc)
    {
        OnApplyChildClip();
        base.OnRender(dc);
    }

    public override UIElement Child
    {
        get => base.Child;
        set
        {
            if (Child is not null)
            {
                Child.SetValue(ClipProperty, _oldClip);
            }
            _oldClip = value is not null ? value.ReadLocalValue(ClipProperty) : null;
            base.Child = value;
        }
    }

    protected virtual void OnApplyChildClip()
    {
        var child = Child;
        if (child is not null)
        {
            _clipRectangle.RadiusX = _clipRectangle.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - (BorderThickness.Left * 0.5));
            _clipRectangle.Rect = new Rect(child.RenderSize);
            child.Clip = _clipRectangle;
        }
    }
}
