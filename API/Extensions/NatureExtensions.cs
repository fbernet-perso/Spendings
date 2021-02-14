using System;
using Core.Model;

namespace API.Extensions
{
    /// <summary>
    /// Adapter for nature
    /// </summary>
    public static class NatureExtensions
    {
        public static Nature? ToNature(this string natureAsString)
        {
            Nature returnValue;
            if (Enum.TryParse<Nature>(natureAsString, true, out returnValue))
            {
                return returnValue;
            }

            return null;
        }
    }
}
