using Ledger8.Common;

using System;

namespace Ledger8.DesktopUI.Models;

public class ImportAccountTypeResult : NotifyBase, IEquatable<ImportAccountTypeResult>
{
    private Guid _id;
    public Guid Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    private string? _result;
    public string Result
    {
        get => _result!;
        set => SetProperty(ref _result, value);
    }

    public ImportAccountTypeResult()
    {
        Id = Guid.NewGuid();
        Description = string.Empty;
        Result = string.Empty;
    }

    public override string ToString() => $"{Description}: {Result}";

    public override bool Equals(object? obj) => obj is ImportAccountTypeResult result && result.Id == Id;

    public bool Equals(ImportAccountTypeResult? result) => result is not null && result.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(ImportAccountTypeResult left, ImportAccountTypeResult right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(ImportAccountTypeResult left, ImportAccountTypeResult right) => !(left == right);
}
