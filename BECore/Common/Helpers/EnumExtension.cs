using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Helpers
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var res = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            if (res == null)
            {
                return null;
            }
            return res.GetCustomAttribute<DisplayAttribute>().GetName();
        }
    }
}
