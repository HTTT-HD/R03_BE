using System;

namespace Common.Helpers
{
    public static class GuidExtension
    {
        public static string ToLowerString(this Guid guid)
        {
            return guid.ToString().ToLower();
        }
        public static string ToLowerString(this Guid? guid)
        {
            if (guid == null) return null;

            return guid.ToString().ToLower();
        }
    }
}
