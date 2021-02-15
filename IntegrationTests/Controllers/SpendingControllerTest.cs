using System;
using System.Collections.Generic;
using API.Controllers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Xunit;

namespace IntegrationTests.Controllers
{
    public class SpendingControllerTest
    {
        public SpendingControllerTest()
        {
            InitializeDatabase();
        }

        [Fact]
        public void ListOrderedWithWrongOrderBy_Fails()
        {
            string wrongOrderBy = "loki";
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                // Act
                BadRequestResult actionResult = sut.ListByUserId(1, wrongOrderBy) as BadRequestResult;

                // Assert 
                Assert.Equal(400, actionResult.StatusCode);
            }
        }

        [Fact]
        public void ListOrderedByUserId()
        {
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                // Act
                ObjectResult actionResult = sut.ListByUserId(1, "Date") as ObjectResult;


                // Assert
                IEnumerable<SpendingDto> spendings = (IEnumerable<SpendingDto>)actionResult.Value;
                Assert.Empty(spendings);
            }
        }

        [Fact]
        public void AddSpendingAndListIt()
        {
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                // Act
                IStatusCodeActionResult actionResult = sut.AddSpending(1, DateTime.UtcNow.AddDays(-1), 100, "USD", "Misc", "Armor") as IStatusCodeActionResult;

                // Assert
                Assert.Equal(200, actionResult.StatusCode);
                ObjectResult listResult = sut.ListByUserId(1, "Date") as ObjectResult;
                IEnumerable<SpendingDto> spendings = (IEnumerable<SpendingDto>)listResult.Value;
                Assert.Single(spendings);
            }
        }

        [Fact]
        public void AddSpendingMissingUser_Fails()
        {
            int missingUserId = 42;
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                // Act
                BadRequestResult actionResult = sut.AddSpending(missingUserId, DateTime.UtcNow.AddDays(-1), 100, "USD", "Misc", "Armor") as BadRequestResult;

                // Assert
                Assert.Equal(400, actionResult.StatusCode);
            }
        }

        [Fact]
        public void AddSpendingTwice_Fails()
        {
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                sut.AddSpending(1, DateTime.UtcNow.AddDays(-1), 100, "USD", "Misc", "Armor");
            }
            // Arrange
            using (SpendingContext spendingContext = new SpendingContext())
            {
                SpendingRepository spendingRepository = new SpendingRepository(spendingContext);
                SpendingController sut = new SpendingController(spendingRepository);

                // Act
                BadRequestResult actionResult = sut.AddSpending(1, DateTime.UtcNow.AddDays(-1), 100, "USD", "Misc", "Armor") as BadRequestResult;
                Assert.Equal(400, actionResult.StatusCode);
            }
        }

        private void InitializeDatabase()
        {
            using (SpendingContext spendingContext = new SpendingContext())
            {
                spendingContext.Database.EnsureDeleted();
                spendingContext.Database.EnsureCreated();
                spendingContext.SaveChanges();
            }
        }
    }
}
