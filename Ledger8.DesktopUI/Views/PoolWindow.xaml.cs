using Ledger8.DesktopUI.ViewModels;

using System;
using System.Windows;

namespace Ledger8.DesktopUI.Views;
/// <summary>
/// Interaction logic for PoolWindow.xaml
/// </summary>
public partial class PoolWindow : Window
{
    public PoolWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not null)
        {
            ((PoolViewModel)DataContext).FocusRequested += NameFocus;
            ((PoolViewModel)DataContext).AmountFocusRequested += AmountFocus;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (DataContext is not null)
        {
            ((PoolViewModel)DataContext).FocusRequested -= NameFocus;
            ((PoolViewModel)DataContext).AmountFocusRequested -= AmountFocus;
        }
    }

    private void NameFocus(object? sender, EventArgs e) => TBName.Focus();

    private void AmountFocus(object? sender, EventArgs e) => TBAmount.Focus();
}
