namespace Ledger8.Common;

public static class Constants
{
    public const double ProductVersion = 8.00;
    public const string ProductName = "Ledger";

    public const double MinumumIconHeight = 16.0;
    public const double DefaultIconHeight = 24.0;
    public const double MaximumIconHeight = 32.0;

    public const int HashLength = 64;           // bytes
    public const int Iterations = 50000;
    public const int NameLength = 50;
    public const int SaltLength = 64;           // ditto
    public const int UriLength = 256;

    // Error exit codes, next = 908

    public const int MigrationFailed = 901;
    public const int CompaniesLoadFailed = 902;
    public const int AccountsLoadFailed = 903;
    public const int TransactionsLoadFailed = 904;
    public const int IdentitiesLoadFailed = 905;
    public const int NoPasswordEntered = 906;
    public const int NoSettings = 907;

    public const string Alt0 = "Alt0";
    public const string Alt1 = "Alt1";
    public const string Background = "Background";
    public const string Border = "Border";
    public const string Checkmark = "/resources/checkmark-32.png";
    public const string ConfigurationFilename = "appsettings.json";
    public const string ConnectionStringName = "Default";
    public const string Count = "Count";
    public const string Date = "date";
    public const string DateTime = "datetime2";
    public const string DBE = "Database Error";
    public const string DuplicateKey = "An item with the same key has already been added.";
    public const string Foreground = "Foreground";
    public const string Indexer = "Item[]";
    public const string IOE = "I/O Error";
    public const string Keys = "Keys";
    public const string MoneyFormat = "decimal(12,2)";
    public const string SaltHashFormat = "binary(64)";
    public const string Values = "Values";
    public const string VarBinary = "varbinary(max)";
    public const string VarBinary64 = "varbinary(64)";


}
