using System.Net;

namespace Ledger8.Common.Interfaces;

public interface IHttpStatusCodeTranslator
{
    string Translate(int code);
    string Translate(HttpStatusCode code);
}
