using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        public int UserId { get; private set; }

        /// <summary>
        /// Family name of the user
        /// </summary
        [Required]
        public string LastName { get; private set; }
        /// <summary>
        /// First name of the user
        /// </summary>
        [Required]
        public string FirstName { get; private set; }

        /// <summary>
        /// Three-character ISO 4217 currency symbol of spendings made by the user
        /// </summary>       
        [Required]
        [MaxLength(3)]
        public string ISOCurrencySymbol { get; private set; }


        protected User()
        {

        }

        public User(string lastName, string firstName, string isoCurrencySymbol)
        {
            this.LastName = lastName;
            this.FirstName = firstName;
            this.ISOCurrencySymbol = isoCurrencySymbol;
        }

        /// <summary>
        /// List the users needed to seed any new application 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> SeedUsers() => new List<User>(){
            new User() { UserId = 1, LastName = "Stark", FirstName = "Anthony", ISOCurrencySymbol = new RegionInfo("US").ISOCurrencySymbol },
            new User(){ UserId = 2, LastName = "Romanov", FirstName = "Natacha", ISOCurrencySymbol = new RegionInfo("RU").ISOCurrencySymbol }
        };

    }
}
