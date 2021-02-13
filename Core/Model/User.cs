using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    /// <summary>
    /// User owning spendings
    /// </summary>
    public class User
    {
        /// <summary>
        /// Technical identifier of the user
        /// Pas demandé dans l'exercice mais cela me parait plus réaliste que d'esperer qu'on n'aura jamais d'homonyme :p
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Family name of the user
        /// </summary
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// First name of the user
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Three-character ISO 4217 currency symbol of spendings made by the user
        /// </summary>       
        [Required]
        [MaxLength(3)]
        public string ISOCurrencySymbol { get; set; }
    }
}
