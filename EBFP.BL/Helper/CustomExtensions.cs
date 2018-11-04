using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBFP.BL.Helper
{
  public static  class CustomExtensions
    {
        public static List<System.Web.Mvc.SelectListItem> ToSelectList(this Enum enumValue, bool withSelected = false)
        {
            var items = new List<System.Web.Mvc.SelectListItem>();

            foreach (Enum e in Enum.GetValues(enumValue.GetType()))
            {
                int enuValue = (int)Enum.Parse(enumValue.GetType(), e.ToString());
                items.Add(new System.Web.Mvc.SelectListItem
                {
                    Selected = withSelected && e.Equals(enumValue),
                    Text = e.ToDescription(),
                    Value = enuValue.ToString()
                });
            }
            
            return items;
        }

        public static decimal Value(this decimal? value)
        {
            return (value.HasValue ? value.Value : 0);
        }

        public static string ToDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            input = input.ToLower();

            var str = string.Empty;

            var count = 0;
            foreach (var item in input.Trim().Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    str = str + (count > 0 ? " " : "") + item.First().ToString().ToUpper() + String.Join("", item.Skip(1));
                }
                count++;
            }

            return str;
        }

        ///
        //public static string ToDesc(this Enum enumValue)
        //{
        //    FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

        //    if (null != fi)
        //    {
        //        object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
        //        if (attrs != null && attrs.Length > 0)
        //            return ((DescriptionAttribute)attrs[0]).Description;
        //    }

        //    return enumValue.ToString();
        //}

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
