using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Model;

namespace Core.Services
{
    /// <summary>
    /// Spending service
    /// </summary>
    public class SpendingService : ISpendingService
    {
        private readonly ISpendingRepository _spendingRepository = null;
        private readonly IUserRepository _userRepository = null;

        public SpendingService(ISpendingRepository spendingRepository, IUserRepository userRepository)
        {
            this._spendingRepository = spendingRepository;
            this._userRepository = userRepository;
        }


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
        public SpendingCreationVerificationError AddSpending(int userId, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, Nature nature, string comment)
        {
            User user = this._userRepository.LoadUser(userId);
            SpendingCreationVerificationError verificationResult = this.VerifySpendingBeforeCreation(user, dateInUtc, amount, isoCurrencySymbol, comment);

            if (verificationResult == SpendingCreationVerificationError.None)
            {
                Spending spending = this.BuildSpending(userId, dateInUtc, amount, isoCurrencySymbol, nature, comment);
                this._spendingRepository.AddSpending(spending);
            }

            return verificationResult;
        }

        /// <summary>
        /// Gives the list a user spending in a specified ordre
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="order">Property by which we want to order </param>
        /// <returns>A list in the specifed order</returns>
        public IEnumerable<Spending> ListOrdered(int userId, SpendingSortOrder order)
        {
            IEnumerable<Spending> spendings = _spendingRepository.ListByUser(userId);

            if (order == SpendingSortOrder.ByAmount)
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
        /// Checks if the data we have are correct for a spending creation
        /// </summary>
        /// <param name="dateInUtc">Date of the spending</param>
        /// <param name="amount">Amount of the spending</param>
        /// <param name="isoCurrencySymbol">Three-character ISO 4217 currency symbol of the spending</param>
        /// <param name="comment">Description of the comment</param>
        /// <returns>A flag indicating errors (if any)</returns>
        private SpendingCreationVerificationError VerifySpendingBeforeCreation(User user, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, string comment)
        {
            SpendingCreationVerificationError returnValue = SpendingCreationVerificationError.None;
            if (dateInUtc.IsInTheFuture())
            {
                returnValue |= SpendingCreationVerificationError.SpendingDateInTheFuture;
            }
            if (dateInUtc.IsExpired())
            {
                returnValue |= SpendingCreationVerificationError.SpendingDateExpired;
            }
            if (string.IsNullOrEmpty(comment))
            {
                returnValue |= SpendingCreationVerificationError.MissingOrEmptyComment;
            }
            if (isoCurrencySymbol != user.ISOCurrencySymbol)
            {
                returnValue |= SpendingCreationVerificationError.SpendingCurrencyDiscrepancy;
            }
            if (amount <= 0)
            {
                returnValue |= SpendingCreationVerificationError.SpendingAmountBelow0;
            }

            return returnValue;
        }


        // On pourrait considerer qu'il faudrait sortir cette methode dans une classe dédiée.
        // L'exemple étant simple, je n'ai pas poussé l'exercice jusque là
        private Spending BuildSpending(int userId, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, Nature nature, string comment)
        {
            Spending returnValue = new Spending();

            returnValue.UserId = this._userRepository.LoadUser(userId).UserId;
            returnValue.Amount = amount;
            returnValue.Comment = comment;
            returnValue.DateInUtc = dateInUtc.Date;
            returnValue.ISOCurrencySymbol = isoCurrencySymbol;
            returnValue.Nature = nature;

            return returnValue;
        }
    }
}
