namespace Ledger8.DesktopUI.Interfaces;

public interface IPasswordManager
{
    void Set(string password);
    string Get();
}
