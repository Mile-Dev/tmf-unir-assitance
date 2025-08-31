using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo == null)
        {
            return value.ToString();
        }

        var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .Cast<DescriptionAttribute>()
                                 .FirstOrDefault();

        return attribute?.Description ?? value.ToString();
    }

    public static T FromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                return (T)field.GetValue(null)!;

            if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                return (T)field.GetValue(null)!;
        }

        throw new ArgumentException($"No matching enum value for description '{description}' in {typeof(T).Name}");
    }
}

