using System;

namespace API.Controllers
{
    /// <summary>
    /// Spending for webapi
    /// </summary>
    public class SpendingDto
    {
        /// <summary>
        /// First name of the user who made the spending
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Ladt name of the user who made the spending
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        /// When was the spending made in utc time
        /// </summary>
        public DateTime DateInUtc { get; set; }

        /// <summary>
        /// Amount of the spending
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Three-character ISO 4217 currency symbol of this spending
        /// </summary>  
        public string ISOCurrencySymbol { get; set; }

        /// <summary>
        /// Nature of the spending
        /// </summary>
        public string Nature { get; set; }

        /// <summary>
        /// Description of the spending
        /// </summary>
        public string Comment { get; set; }
    }
}
