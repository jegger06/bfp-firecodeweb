namespace EBFP.Utils
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    public static class Extensions
    {
        public static string ToDesc(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumValue.ToString();
        }

        public static string TrimNullable(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";

            return value.Trim();
        }

        public static int ToInt(this string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public static bool NullableToBool(this bool? value)
        {
            return (value.HasValue ? value.Value : false);
        }

        public static double NullableToDouble(this decimal? value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(this string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }
    }
}
