using Ledger8.DesktopUI.ViewModels;

using System;
using System.Windows;

namespace Ledger8.DesktopUI.Views;
/// <summary>
/// Interaction logic for AccountTypeWindow.xaml
/// </summary>
public partial class AccountTypeWindow : Window
{
    public AccountTypeWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e) => ((AccountTypeViewModel)DataContext).FocusRequested += Focus;

    private void Window_Unloaded(object sender, RoutedEventArgs e) => ((AccountTypeViewModel)DataContext).FocusRequested -= Focus;

    private void Focus(object? sender, EventArgs e) => TBDescription.Focus();
}
