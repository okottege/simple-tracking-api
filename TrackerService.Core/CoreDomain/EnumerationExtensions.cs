using System;

namespace TrackerService.Core.CoreDomain
{
    public static class EnumerationExtensions
    {
        public static T FromString<T>(string strValue) where T : struct
        {
            if (Enum.TryParse(strValue, true, out T enumValue))
            {
                return enumValue;
            }

            throw new NotSupportedException($"Could not convert '{strValue}' to enum type '{typeof(T).Name}'.");
        }
    }
}
