using System;

namespace Core.Model
{
    /// <summary>
    /// Creation verification result
    /// </summary>
    [Flags]
    public enum SpendingCreationVerificationError
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
        // J'ai ajouté ce cas car il me paraissait bizarre 
        SpendingAmountBelow0 = 32

        // Un utilisateur ne peut pas déclarer deux fois la même dépense (même date et même montant),
        // Non implementé ici car il me semble que c'est assuré de maniere plus performante par la PK dans la Db.
        // On peut discuter ce choix car
        //   par exemple dans du web cela risque d'emettre une erreur 500 alors que les erreurs ci dessus vont probablement emettre du 400
        //   cela creer une dependance "cachee" sur le stockage
    }

}
