using System;
using System.Collections.Generic;
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

        public SpendingService(ISpendingRepository spendingRepository)
        {
            this._spendingRepository = spendingRepository;
        }

        /// <summary>
        /// List a user spending in the given order
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="orderBy">Property by which we want to order </param>
        /// <returns>The list of spending for the given user</returns>
        public IEnumerable<Spending> ListByUserId(int userId, SortSpendingBy orderBy)
        {
            return this._spendingRepository.ListByUserId(userId, orderBy);
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
        public SpendingCreationError TryCreateSpending(int userId, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, Nature nature, string comment)
        {
            User user = this._spendingRepository.LoadUserById(userId);
            DateTime meaningfullDate = ExtractDate(dateInUtc);

            SpendingCreationError verificationResult = VerifySpendingBeforeCreation(user, meaningfullDate, amount, isoCurrencySymbol, comment);

            if (verificationResult == SpendingCreationError.None)
            {
                Spending spendingToCreate = new Spending();
                spendingToCreate.User = user;
                spendingToCreate.UserId = user.UserId;

                spendingToCreate.Amount = amount;
                spendingToCreate.Comment = comment;
                spendingToCreate.DateInUtc = meaningfullDate;
                spendingToCreate.ISOCurrencySymbol = isoCurrencySymbol;
                spendingToCreate.Nature = nature;

                this._spendingRepository.AddSpending(spendingToCreate);
            }

            return verificationResult;
        }

        /// <summary>
        /// Checks if the data we have are correct for a spending creation
        /// </summary>
        /// <param name="user">User owning the spending we want to create</param>
        /// <param name="dateInUtc">Date of the spending</param>
        /// <param name="amount">Amount of the spending</param>
        /// <param name="isoCurrencySymbol">Three-character ISO 4217 currency symbol of the spending</param>
        /// <param name="comment">Description of the comment</param>
        /// <returns>A flag indicating errors (if any)</returns>
        private SpendingCreationError VerifySpendingBeforeCreation(User user, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, string comment)
        {
            SpendingCreationError returnValue = SpendingCreationError.None;
            if (dateInUtc.IsInTheFuture())
            {
                returnValue |= SpendingCreationError.SpendingDateInTheFuture;
            }
            if (dateInUtc.IsExpired())
            {
                returnValue |= SpendingCreationError.SpendingDateExpired;
            }
            if (string.IsNullOrEmpty(comment))
            {
                returnValue |= SpendingCreationError.MissingOrEmptyComment;
            }
            if (amount <= 0)
            {
                returnValue |= SpendingCreationError.SpendingAmountBelow0;
            }

            if (user == null)
            {
                returnValue |= SpendingCreationError.UserNotFound;
            }

            if (user != null && isoCurrencySymbol != user?.ISOCurrencySymbol)
            {
                returnValue |= SpendingCreationError.SpendingCurrencyDiscrepancy;
            }

            if (user != null && this._spendingRepository.SpendingExists(user.UserId, dateInUtc, amount))
            {
                returnValue |= SpendingCreationError.DuplicateSpending;
            }

            return returnValue;
        }

        /// <summary>
        /// Extract the meaningfull part of a date
        /// </summary>
        /// <param name="dateinUtc">Spending date in utc</param>
        /// <returns>The neaningfull part of a date</returns>
        private static DateTime ExtractDate(DateTime dateInUtc)
        {
            return dateInUtc.Date;// Il me semble que seule la date est significative pour une depense...
        }
    }
}
