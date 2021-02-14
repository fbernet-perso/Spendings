using System;
using API.Controllers;
using Core.Model;

namespace API.Extensions
{
    /// <summary>
    /// Adapter for sSpendingDto
    /// </summary>
    public static class SpendingDtoExtensions
    {
        // Fait manuellement ici mais pour un gros projet une librairie comme automapper ou a minima l'extension MappingGenerator serait plus pertinent.
        public static SpendingDto ToSpendingDto(this Spending spending)
        {
            SpendingDto returnValue = new SpendingDto();

            returnValue.Amount = spending.Amount;
            returnValue.Comment = spending.Comment;
            returnValue.DateInUtc = spending.DateInUtc;
            returnValue.ISOCurrencySymbol = spending.ISOCurrencySymbol;
            returnValue.Nature = Enum.GetName(typeof(Nature), spending.Nature);
            returnValue.UserFirstName = spending.User.FirstName;
            returnValue.UserLastName = spending.User.LastName;

            return returnValue;
        }
    }
}
