using System.Data;

namespace Ledger8.Common;

public sealed class DatabaseInfo
{
    public string? Name { get; set; }
    public double Size { get; set; }
    public double Unallocated { get; set; }
    public double Reserved { get; set; }
    public double Data { get; set; }
    public double IndexSize { get; set; }
    public double Unused { get; set; }

    public override string ToString() => Name ?? string.Empty;

    public DatabaseInfo()
    {
        Name = string.Empty;
        Size = 0;
        Unallocated = 0;
        Reserved = 0;
        Data = 0;
        IndexSize = 0;
        Unused = 0;
    }

    public DatabaseInfo(DataSet dataset) : this()
    {
        if (dataset is null)
        {
            throw new ArgumentNullException(nameof(dataset));
        }
        Name = dataset.Tables[0].Rows[0][0] as string;
        Size = Parse(dataset.Tables[0].Rows[0][1] as string);
        Unallocated = Parse(dataset.Tables[0].Rows[0][2] as string);
        Reserved = Parse(dataset.Tables[1].Rows[0][0] as string);
        Data = Parse(dataset.Tables[1].Rows[0][1] as string);
        IndexSize = Parse(dataset.Tables[1].Rows[0][2] as string);
        Unused = Parse(dataset.Tables[1].Rows[0][3] as string);
    }

    private readonly static Dictionary<string, double> _multipliers = new()
    {
        ["KB"] = 1_000.0,
        ["MB"] = 1_000_000.0,
        ["GB"] = 1_000_000_000.0,
        ["TB"] = 1_000_000_000_000.0
    };

    private static double Parse(string? spec)
    {
        double ret = 0;

        if (string.IsNullOrEmpty(spec))
        {
            return 0;
        }

        var parts = spec.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            if (!double.TryParse(spec, out ret))
            {
                ret = 0;
            }
        }
        else if (parts.Length == 2)
        {
            if (double.TryParse(parts[0], out var d))
            {
                if (_multipliers.ContainsKey(parts[1]))
                {
                    d *= _multipliers[parts[1]];
                }
                ret = d;
            }
        }
        return ret;
    }
}
