namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<TEnum> Flags<TEnum>(this TEnum @enum)
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Where(e => @enum.HasFlag(e));
    }
}