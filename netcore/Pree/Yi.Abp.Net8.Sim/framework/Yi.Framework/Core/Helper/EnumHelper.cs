namespace Yi.Framework.Core.Helper;

public static class EnumHelper
{
    public static New EnumToEnum<New>(this object oldEnum)
    {
        if (oldEnum is null) throw new ArgumentNullException(nameof(oldEnum));
        return (New)Enum.ToObject(typeof(New), oldEnum.GetHashCode());
    }

    public static TEnum StringToEnum<TEnum>(this string str)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), str);
    }
}