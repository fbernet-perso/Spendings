using Core.Model;

namespace Core.Interfaces
{
    /// <summary>
    /// User repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Load a user 
        /// </summary>
        /// <param name="userId">Id of the user we want</param>
        /// <returns>The specified user</returns>
        User LoadUser(int userId);
    }
}
