using System;
using System.Collections.Generic;
using Core.Model;

namespace Core.Interfaces
{
    // Un seul repository car je considere dans l'exemple que User et spending sont dans le meme aggregate.
    /// <summary>
    /// Spending repository
    /// </summary>
    public interface ISpendingRepository
    {
        /// <summary>
        /// Add a spending
        /// </summary>
        /// <param name="spending">Spending to add</param>
        void AddSpending(Spending spending);

        /// <summary>
        /// List a user spending in the given order
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="order">Property by which we want to order </param>
        /// <returns>The list of spending for the given user</returns>
        IEnumerable<Spending> ListByUserId(int userId, SortSpendingBy order);

        /// <summary>
        /// Load a user 
        /// </summary>
        /// <param name="userId">Id of the user we want</param>
        /// <returns>The specified user</returns>
        User LoadUserById(int userId);

        /// <summary>
        /// Checks if a spending already exists
        /// </summary>
        /// <param name="userId">Id of the user owning the spending</param>
        /// <param name="dateInUtc">Date of the spending in utc</param>
        /// <param name="amount">Amount of the spending</param>
        /// <returns>If the spending was found or not</returns>
        bool SpendingExists(int userId, DateTime dateInUtc, decimal amount);
    }
}
