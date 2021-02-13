using System.Collections.Generic;
using Core.Model;

namespace Core.Interfaces
{
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
        /// List a user spending
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <returns>The list of spending for the given user</returns>
        IEnumerable<Spending> ListByUser(int userId);
    }
}
