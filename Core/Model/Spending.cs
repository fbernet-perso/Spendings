using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    /// <summary>
    /// A spending for an user
    /// </summary>
    public class Spending
    {
        /// <summary>
        /// User owning the spending
        /// </summary>
        [Required]
        virtual public User User { get; internal set; }
        [Required]
        public int UserId { get; internal set; }

        /// <summary>
        /// When was the spending made in utc time
        /// </summary>
        [Required]
        public DateTime DateInUtc { get; internal set; }

        /// <summary>
        /// Amount of the spending
        /// </summary>
        [Required]
        public decimal Amount { get; internal set; }

        /// <summary>
        /// Three-character ISO 4217 currency symbol of this spending
        /// </summary>  
        [Required]
        [MaxLength(3)]
        public string ISOCurrencySymbol { get; internal set; }

        /// <summary>
        /// Nature of the spending
        /// </summary>
        [Required]
        public Nature Nature { get; internal set; }

        /// <summary>
        /// Description of the spending
        /// </summary>
        [Required]
        public string Comment { get; internal set; }

        internal protected Spending()
        {

        }
    }
}
