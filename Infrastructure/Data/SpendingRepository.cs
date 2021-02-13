using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Model;

namespace Infrastructure.Data
{
    /// <summary>
    /// Spending repository
    /// </summary>
    public class SpendingRepository : ISpendingRepository
    {
        /// <summary>
        /// Add a spending
        /// </summary>
        /// <param name="spending">Spending to add</param>
        public void AddSpending(Spending spending)
        {
            SpendingContext db = new SpendingContext();
            db.Spendings.Add(spending);

            db.SaveChanges();
        }

        /// <summary>
        /// List a user spending
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <returns>The list of spending for the given user</returns>
        public IEnumerable<Spending> ListByUser(int userId)
        {
            SpendingContext db = new SpendingContext();
            return db.Spendings.Where(s => s.UserId == userId);
        }
    }
}
