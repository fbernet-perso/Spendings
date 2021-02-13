using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Model;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Data
{
    public class SpendingServiceTest
    {
        public SpendingServiceTest()
        {
            InitializeDatabase();
        }

        [Fact]
        public void ListOrdered()
        {
            SpendingRepository spendingRepository = new SpendingRepository();
            UserRepository userRepository = new UserRepository();
            SpendingService sut = new SpendingService(spendingRepository, userRepository);

            IEnumerable<Spending> spendings = sut.ListOrdered(1, SpendingSortOrder.ByAmount);

            Assert.Equal(3, spendings.Count());
            Spending spending = spendings.First();
            Assert.Equal("Anthony, Stark", $"{spending.User.FirstName}, {spending.User.LastName}");
        }

        [Fact]
        public void AddSpending()
        {
            SpendingRepository spendingRepository = new SpendingRepository();
            UserRepository userRepository = new UserRepository();
            SpendingService sut = new SpendingService(spendingRepository, userRepository);

            SpendingCreationVerificationError errors = sut.AddSpending(3, DateTime.UtcNow, 42, "NOK", Nature.Misc, "Hammer");

            Assert.Equal(SpendingCreationVerificationError.None, errors);
            string mojlnir = sut.ListOrdered(3, SpendingSortOrder.ByAmount)
                .Single()
                .Comment;

            Assert.Equal("Hammer", mojlnir);
        }

        [Fact]
        public void AddSpendingMissingUser_ThrowsException()
        {
            SpendingRepository spendingRepository = new SpendingRepository();
            UserRepository userRepository = new UserRepository();
            SpendingService sut = new SpendingService(spendingRepository, userRepository);

            Assert.Throws<InvalidOperationException>(() => sut.AddSpending(4, DateTime.UtcNow, 1, "USD", Nature.Misc, "Purple pants"));
        }

        [Fact]
        public void AddSpendingTwice_ThrowsException()
        {
            SpendingRepository spendingRepository = new SpendingRepository();
            UserRepository userRepository = new UserRepository();
            SpendingService sut = new SpendingService(spendingRepository, userRepository);

            DateTime spendingDateTime = DateTime.UtcNow.AddDays(-1);
            SpendingCreationVerificationError errors = sut.AddSpending(3, spendingDateTime, 42, "NOK", Nature.Misc, "Hammer");
            Assert.Equal(SpendingCreationVerificationError.None, errors);

            Assert.Throws<DbUpdateException>(() => sut.AddSpending(3, spendingDateTime, 42, "NOK", Nature.Misc, "Hammer"));
        }

        private void InitializeDatabase()
        {
            using (SpendingContext db = new SpendingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Users.Add(new User() { LastName = "Blake", FirstName = "Donald", ISOCurrencySymbol = new RegionInfo("NO").ISOCurrencySymbol });

                db.Spendings.Add(new Spending() { Amount = 1, Comment = "1", DateInUtc = new DateTime(2021, 1, 1), ISOCurrencySymbol = "USD", Nature = Nature.Misc, UserId = 1 });
                db.Spendings.Add(new Spending() { Amount = 2, Comment = "2", DateInUtc = new DateTime(2021, 1, 2), ISOCurrencySymbol = "USD", Nature = Nature.Hotel, UserId = 1 });
                db.Spendings.Add(new Spending() { Amount = 3, Comment = "3", DateInUtc = new DateTime(2021, 1, 3), ISOCurrencySymbol = "USD", Nature = Nature.Restaurant, UserId = 1 });
                db.Spendings.Add(new Spending() { Amount = 4, Comment = "4", DateInUtc = new DateTime(2021, 1, 4), ISOCurrencySymbol = "RUB", Nature = Nature.Misc, UserId = 2 });
                db.Spendings.Add(new Spending() { Amount = 5, Comment = "5", DateInUtc = new DateTime(2021, 1, 5), ISOCurrencySymbol = "RUB", Nature = Nature.Misc, UserId = 2 });

                db.SaveChanges();
            }
        }
    }
}
