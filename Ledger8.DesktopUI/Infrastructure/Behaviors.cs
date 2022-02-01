using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ledger8.DesktopUI.Infrastructure;

public class Behaviors
{

    // Drop

    public readonly static DependencyProperty DropBehaviorProperty =
            DependencyProperty.RegisterAttached("DropBehavior", typeof(ICommand),
            typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
            OnDropBehaviorChanged));

    public static ICommand GetDropBehavior(DependencyObject d) => (ICommand)d.GetValue(DropBehaviorProperty);
    public static void SetDropBehavior(DependencyObject d, ICommand value) =>
        d.SetValue(DropBehaviorProperty, value);
    public static void OnDropBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            element.Drop += (s, a) =>
            {
                var command = GetDropBehavior(element);
                if (command != null)
                {
                    if (command.CanExecute(a.Data))
                    {
                        command.Execute(a.Data);
                    }
                }
            };
        }
    }

    // MouseLeftButtonUp

    public readonly static DependencyProperty MouseLeftButtonUpBehaviorProperty =
            DependencyProperty.RegisterAttached("MouseLeftButtonUpBehavior",
            typeof(ICommand), typeof(Behaviors), new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.None, OnMouseLeftButtonUpBehaviorChanged));

    public static ICommand GetMouseLeftButtonUpBehavior(DependencyObject d) =>
        (ICommand)d.GetValue(MouseLeftButtonUpBehaviorProperty);
    public static void SetMouseLeftButtonUpBehavior(DependencyObject d, ICommand value) =>
        d.SetValue(MouseLeftButtonUpBehaviorProperty, value);
    public static void OnMouseLeftButtonUpBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            element.MouseLeftButtonUp += (s, a) =>
            {
                var command = GetMouseLeftButtonUpBehavior(element);
                if (command != null)
                {
                    if (command.CanExecute(a))
                    {
                        command.Execute(a);
                    }
                }
            };
        }
    }

    // MouseDoubleClick

    public readonly static DependencyProperty MouseDoubleClickBehaviorProperty =
            DependencyProperty.RegisterAttached("MouseDoubleClickBehavior",
            typeof(ICommand), typeof(Behaviors), new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.None, OnMouseDoubleClickBehaviorChanged));

    public static ICommand GetMouseDoubleClickBehavior(DependencyObject d) =>
        (ICommand)d.GetValue(MouseDoubleClickBehaviorProperty);
    public static void SetMouseDoubleClickBehavior(DependencyObject d, ICommand value) =>
        d.SetValue(MouseDoubleClickBehaviorProperty, value);
    public static void OnMouseDoubleClickBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Control control)
        {
            control.MouseDoubleClick += (s, a) =>
            {
                var command = GetMouseDoubleClickBehavior(control);
                if (command != null)
                {
                    if (command.CanExecute(a))
                    {
                        command.Execute(a);
                    }
                }
            };
        }
    }

    // Window Loaded

    public readonly static DependencyProperty WindowLoadedBehaviorProperty =
            DependencyProperty.RegisterAttached("WindowLoadedBehavior", typeof(ICommand),
            typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
            OnWindowLoadedBehaviorChanged));

    public static ICommand GetWindowLoadedBehavior(DependencyObject d) =>
        (ICommand)d.GetValue(WindowLoadedBehaviorProperty);
    public static void SetWindowLoadedBehavior(DependencyObject d, ICommand value) =>
        d.SetValue(WindowLoadedBehaviorProperty, value);
    public static void OnWindowLoadedBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window w)
        {
            w.Loaded += (s, a) =>
            {
                var command = GetWindowLoadedBehavior(w);
                if (command != null)
                {
                    if (command.CanExecute(a))
                    {
                        command.Execute(a);
                    }
                }
            };
        }
    }

    // Window Closed

    public readonly static DependencyProperty WindowClosedBehaviorProperty =
            DependencyProperty.RegisterAttached("WindowClosedBehavior", typeof(ICommand),
            typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
            OnWindowClosedBehaviorChanged));

    public static ICommand GetWindowClosedBehavior(DependencyObject d) =>
        (ICommand)d.GetValue(WindowClosedBehaviorProperty);
    public static void SetWindowClosedBehavior(DependencyObject d, ICommand value) =>
        d.SetValue(WindowClosedBehaviorProperty, value);
    public static void OnWindowClosedBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window w)
        {
            w.Closed += (s, a) =>
            {
                var command = GetWindowClosedBehavior(w);
                if (command != null)
                {
                    if (command.CanExecute(a))
                    {
                        command.Execute(a);
                    }
                }
            };
        }
    }
}
