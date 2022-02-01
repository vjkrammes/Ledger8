using Ledger8.DesktopUI.ViewModels;

using System.Windows;

namespace Ledger8.DesktopUI.Views;
/// <summary>
/// Interaction logic for IdentityWindow.xaml
/// </summary>
public partial class IdentityWindow : Window
{
    public IdentityWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not null)
        {
            PBPassword1.Password = ((IdentityViewModel)DataContext).Password1;
            PBPassword2.Password = ((IdentityViewModel)DataContext).Password2;
        }
    }

    private void PBPassword1_PasswordChanged(object sender, RoutedEventArgs e) => ((IdentityViewModel)DataContext).Password1 = PBPassword1.Password;

    private void PBPassword2_PasswordChanged(object sender, RoutedEventArgs e) => ((IdentityViewModel)DataContext).Password2 = PBPassword2.Password;
}
