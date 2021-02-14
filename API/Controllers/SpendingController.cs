using System;
using System.Linq;
using API.Extensions;
using Core.Interfaces;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/spendings")]
    [ApiController]
    public class SpendingController : Controller
    {
        private readonly ISpendingService _spendingService;

        public SpendingController(ISpendingRepository spendingRepository)
        {
            this._spendingService = new SpendingService(spendingRepository);
        }

        [Route("{userId:int}")]
        [HttpGet]
        /// <summary>
        /// Gives the list a user spending in a specified order
        /// </summary>
        /// <param name="userId">Id of the user we want the spendings of</param>
        /// <param name="order">Property by which we want to order as a string</param>
        /// <returns>A list in the specifed order</returns>
        public IActionResult ListByUserId(int userId, string orderBy)
        {
            SortSpendingBy? spendingSortOrder = orderBy.ToSpendingSortOrder();

            // Unknown sorting order
            if (spendingSortOrder == null)
            {
                return BadRequest();
            }

            return Ok(this._spendingService
                .ListByUserId(userId, spendingSortOrder.Value)
                .Select(s => s.ToSpendingDto())
                );
        }

        [HttpPut]
        /// <summary>
        /// Add a spending for a user
        /// </summary>
        /// <param name="userId">Id of the user owning the spending we want to create</param>
        /// <param name="dateInUtc">Date of the spending in utc</param>
        /// <param name="amount">Amount of the spending</param>
        /// <param name="isoCurrencySymbol">Three-character ISO 4217 currency symbol of this spending</param>
        /// <param name="nature">Nature of the spending</param>
        /// <param name="comment">Description of the comment</param>
        /// <returns>A 400 error if the request was incorrect, else return 200</returns>
        public IActionResult AddSpending(int userId, DateTime dateInUtc, decimal amount, string isoCurrencySymbol, string nature, string comment)
        {
            Nature? natureEnum = nature.ToNature();

            // Unknown nature
            if (natureEnum == null)
            {
                return BadRequest();
            }

            SpendingCreationError verificationResult = this._spendingService.TryCreateSpending(userId, dateInUtc, amount, isoCurrencySymbol, natureEnum.Value, comment);

            if (verificationResult != SpendingCreationError.None)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
