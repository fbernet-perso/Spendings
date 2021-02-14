using System;
using Core.Model;

namespace API.Extensions
{
    /// <summary>
    /// Adapter for SpendingSortOrder
    /// </summary>
    public static class SpendingSortOrderExtensions
    {
        public static SortSpendingBy? ToSpendingSortOrder(this string spendingSortOrderasString)
        {
            SortSpendingBy returnValue;
            if (Enum.TryParse<SortSpendingBy>(spendingSortOrderasString, true, out returnValue))
            {
                return returnValue;
            }

            return null;
        }
    }
}
