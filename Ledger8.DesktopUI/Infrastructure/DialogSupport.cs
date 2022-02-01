using System;
using System.Windows;

namespace Ledger8.DesktopUI.Infrastructure;

public static class DialogSupport
{
    public static bool? ShowDialog<T>(object viewmodel, Window? owner = null, int minresolution = 0) where T : Window
    {
        var dialog = Activator.CreateInstance(typeof(T)) as Window ?? throw new InvalidOperationException("Failed to create window");
        dialog.DataContext = viewmodel;
        dialog.Owner = owner;
        if (minresolution != 0 && SystemParameters.PrimaryScreenWidth <= minresolution)
        {
            dialog.WindowState = WindowState.Maximized;
        }
        return dialog.ShowDialog();
    }
}

public static class DialogCloser
{
    public readonly static DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult",
        typeof(bool?), typeof(DialogCloser), new FrameworkPropertyMetadata(DialogResultChanged));

    public static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window w)
        {
            w.DialogResult = e.NewValue as bool?;
        }
    }

    public static void SetDialogResult(Window target, bool? value) => target.SetValue(DialogResultProperty, value);
}
