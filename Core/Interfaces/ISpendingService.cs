using System;
using System.Collections.Generic;
using Core.Model;

namespace Core.Interfaces
{
    /// <summary>
    /// Spending service
    /// </summary>
    public interface ISpendingService
    {
        /// <summary>
        /// List a user spending in the given order
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="orderBy">Property by which we want to order </param>
        /// <returns>The list of spending for the given user</returns>
        IEnumerable<Spending> ListByUserId(int userId, SortSpendingBy orderBy);

        /// <summary>
        /// Add a spending for a user
        /// </summary>
        /// <param name="userId">Id of the user owning the spending we want to create</param>
        /// <param name="dateInUtc">Date of the spending in utc</param>
        /// <param name="amount">Amount of the spending</param>
        /// <param name="isoCurrencySymbol">Three-character ISO 4217 currency symbol of this spending</param>
        /// <param name="nature">Nature of the spending</param>
        /// <param name="comment">Description of the comment</param>
        /// <returns>A flag indicating errors (if any) in the query</returns>
        SpendingCreationError TryCreateSpending(int userId, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, Nature nature, string comment);
    }
}
