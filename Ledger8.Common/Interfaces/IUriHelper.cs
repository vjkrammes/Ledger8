namespace Ledger8.Common.Interfaces;

public interface IUriHelper
{
    void SetBase(string path);
    void SetVersion(int version);
    Uri Create(string controller, string action, params object[] parms);
}
