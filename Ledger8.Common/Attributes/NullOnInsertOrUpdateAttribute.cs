namespace Ledger8.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NullOnInsertOrUpdateAttribute : Attribute
{
    public NullOnInsertOrUpdateAttribute() => NullOnInsertOrUpdate = true;
    public NullOnInsertOrUpdateAttribute(bool nullOnInsertOrUpdate) => NullOnInsertOrUpdate = nullOnInsertOrUpdate;
    public bool NullOnInsertOrUpdate { get; }
}
