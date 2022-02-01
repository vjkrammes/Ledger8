namespace Ledger8.Common;

public static class Tools
{
    public static bool Any(params bool[] items) => items is not null && items.Any();

    public static bool All(params bool[] items) => items is not null && items.All(x => x);

    public static T LeastOf<T>(params T[] items) where T : IComparable<T> => items.Min()!;

    public static T GreatestOf<T>(params T[] items) where T : IComparable<T> => items.Max()!;
}
