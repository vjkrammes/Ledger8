namespace Ledger8.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HasNullableMembersAttribute : Attribute
{
    public HasNullableMembersAttribute() => HasNullableMembers = true;
    public HasNullableMembersAttribute(bool hasNullableMembers) => HasNullableMembers = hasNullableMembers;
    public bool HasNullableMembers { get; }
}
