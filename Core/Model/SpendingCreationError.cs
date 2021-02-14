using System;

namespace Core.Model
{
    /// <summary>
    /// Creation verification result
    /// </summary>
    [Flags]
    public enum SpendingCreationError
    {
        None = 0,
        // Une dépense ne peut pas avoir une date dans le futur,
        SpendingDateInTheFuture = 2,
        // Une dépense ne peut pas être datée de plus de 3 mois,
        SpendingDateExpired = 4,
        // Le commentaire est obligatoire,
        MissingOrEmptyComment = 8,
        //La devise de la dépense doit être identique à celle de l'utilisateur. 
        SpendingCurrencyDiscrepancy = 16,
        // Un utilisateur ne peut pas déclarer deux fois la même dépense (même date et même montant),
        DuplicateSpending = 32,

        // J'ai ajouté les cas suivants car ils me paraissaient necessaires
        UserNotFound = 64,
        SpendingAmountBelow0 = 128,

    }

}
