using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Model;

namespace Infrastructure.Data
{
    /// <summary>
    /// Spending repository.
    /// </summary>
    public class SpendingRepository : ISpendingRepository
    {
        private readonly SpendingContext _spendingContext;
        public SpendingRepository(SpendingContext spendingContext)
        {
            this._spendingContext = spendingContext;
        }
        /// <summary>
        /// Add a spending
        /// </summary>
        /// <param name="spending">Spending to add</param>
        public void AddSpending(Spending spending)
        {
            this._spendingContext.Spendings.Add(spending);

            this._spendingContext.SaveChanges();
        }

        /// <summary>
        /// List a user spending in the given order
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="order">Property by which we want to order </param>
        /// <returns>The list of spending for the given user</returns>
        public IEnumerable<Spending> ListByUserId(int userId, SortSpendingBy order)
        {
            IEnumerable<Spending> spendings = this._spendingContext.Spendings.Where(s => s.UserId == userId);

            if (order == SortSpendingBy.Amount)
            {
                spendings = spendings.OrderBy(s => s.Amount);
            }
            else
            {
                spendings = spendings.OrderBy(s => s.DateInUtc);
            }

            return spendings;
        }

        /// <summary>
        /// Load a user 
        /// </summary>
        /// <param name="userId">Id of the user we want</param>
        /// <returns>The specified user</returns>
        public User LoadUserById(int userId)
        {
            return this._spendingContext.Users.SingleOrDefault(u => u.UserId == userId);
        }

        /// <summary>
        /// Checks if a spending already exists
        /// </summary>
        /// <param name="userId">Id of the user owning the spending</param>
        /// <param name="dateInUtc">Date of the spending in utc</param>
        /// <param name="amount">Amount of the spending</param>
        /// <returns>If the spending was found or not</returns>
        public bool SpendingExists(int userId, DateTime dateInUtc, decimal amount)
        {
            return this._spendingContext.Spendings
                .Any(s =>
                s.UserId == userId
                && s.DateInUtc == dateInUtc
                && s.Amount == amount);
        }
    }
}
