using Ledger8.Common.Interfaces;

using System.Net;

namespace Ledger8.Common;

public class HttpStatusCodeTranslator : IHttpStatusCodeTranslator
{
    public string Translate(int code) => code switch
    {
        0 => Strings.Status0,
        400 => Strings.Status400,
        401 => Strings.Status401,
        403 => Strings.Status403,
        404 => Strings.Status404,
        405 => Strings.Status405,
        408 => Strings.Status408,
        429 => Strings.Status429,
        500 => Strings.Status500,
        _ => string.Format(Strings.OtherStatus, code)
    };

    public string Translate(HttpStatusCode code) => Translate((int)code);
}
