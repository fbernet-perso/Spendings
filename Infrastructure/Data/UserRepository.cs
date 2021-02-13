using System.Linq;
using Core.Interfaces;
using Core.Model;

namespace Infrastructure.Data
{
    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Load a user 
        /// </summary>
        /// <param name="userId">Id of the user we want</param>
        /// <returns>The specified user</returns>
        public User LoadUser(int userId)
        {
            SpendingContext db = new SpendingContext();
            return db.Users.Single(u => u.UserId == userId);
        }
    }
}
