using System;

namespace Core.Services
{
    public static class SpendingExtensions
    {
        /// <summary>
        /// Spendings can be declared up to 3 months after being made
        /// Laissé en constante pour l'exercice mais il serait probablement préferable de le rendre parametrable
        /// </summary>
        private static uint _maximumDurationInMonthForDeclaringSpendings = 3;

        public static bool IsInTheFuture(this DateTime dateInUtc)
        {
            return dateInUtc > DateTime.UtcNow;
        }
        public static bool IsExpired(this DateTime dateInUtc)
        {
            int validityInMonth = -1 * (int)_maximumDurationInMonthForDeclaringSpendings;
            DateTime minimumAcceptanceDate = DateTime.UtcNow.AddMonths(validityInMonth);

            return dateInUtc < minimumAcceptanceDate;
        }
    }
}
