using Ledger8.DesktopUI.ViewModels;

using System.Windows;

namespace Ledger8.DesktopUI.Views;
/// <summary>
/// Interaction logic for PasswordWindow.xaml
/// </summary>
public partial class PasswordWindow : Window
{
    public PasswordWindow() => InitializeComponent();

    private void PBPassword1_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is not null)
        {
            ((PasswordViewModel)DataContext).Password1 = PBPassword1.Password;
        }
    }

    private void PBPassword2_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is not null)
        {
            ((PasswordViewModel)DataContext).Password2 = PBPassword2.Password;
        }
    }
}
