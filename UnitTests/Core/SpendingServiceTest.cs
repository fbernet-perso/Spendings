using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Model;
using Core.Services;
using Moq;
using Xunit;

namespace UnitTests.Core
{
    public class SpendingServiceTest
    {
        // Je n'ai pas l'habitude d'utiliser xUnit, donc je ne sais pas si c'est la meilleure manière de faire pour la lisibilité
        [Theory]
        [InlineData(1, 12, 100, "USD", Nature.Misc, "Taxi", SpendingCreationVerificationError.SpendingDateInTheFuture)]
        [InlineData(1, -12, 100, "USD", Nature.Misc, "Taxi", SpendingCreationVerificationError.SpendingDateExpired)]
        [InlineData(1, -1, 100, "USD", Nature.Misc, "", SpendingCreationVerificationError.MissingOrEmptyComment)]
        [InlineData(1, -1, 100, "RUB", Nature.Misc, "Taxi", SpendingCreationVerificationError.SpendingCurrencyDiscrepancy)]
        [InlineData(1, -1, -100, "USD", Nature.Misc, "Taxi", SpendingCreationVerificationError.SpendingAmountBelow0)]
        [InlineData(1, 12, 100, "RUB", Nature.Misc, "", SpendingCreationVerificationError.MissingOrEmptyComment | SpendingCreationVerificationError.SpendingCurrencyDiscrepancy | SpendingCreationVerificationError.SpendingDateInTheFuture)]
        [InlineData(1, -1, 100, "USD", Nature.Misc, "Taxi", SpendingCreationVerificationError.None)]
        public void AddSpending(int userId, int monthToAdd, decimal amount, string currency, Nature nature, string comment, SpendingCreationVerificationError expectedOutcome)
        {
            User userUS = new User() { UserId = 1, FirstName = "Anthony", LastName = "Stark", ISOCurrencySymbol = "USD" };
            Mock<ISpendingRepository> spendingRepositoryMock = new Mock<ISpendingRepository>(MockBehavior.Strict);
            spendingRepositoryMock.Setup(r => r.AddSpending(It.IsAny<Spending>()));

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(r => r.LoadUser(1)).Returns(userUS);


            DateTime spendingDate = DateTime.Now.AddMonths(monthToAdd);
            SpendingService sut = new SpendingService(spendingRepositoryMock.Object, userRepositoryMock.Object);
            SpendingCreationVerificationError outcome = sut.AddSpending(userId, spendingDate, amount, currency, nature, comment);


            Assert.Equal(expectedOutcome, outcome);
            if (expectedOutcome == SpendingCreationVerificationError.None)
            {
                spendingRepositoryMock.Verify(s => s.AddSpending(It.IsAny<Spending>()), Times.Once);
            }
            else
            {
                spendingRepositoryMock.Verify(s => s.AddSpending(It.IsAny<Spending>()), Times.Never);
            }
        }

        [Fact]
        public void ListOrdered_ByAmount()
        {
            List<Spending> sourceSpendings = new List<Spending>()
            {
                new Spending(){Amount=42, Comment="2"},
                new Spending(){Amount=13, Comment="1"},
                new Spending(){Amount=666, Comment="3"}
            };
            Mock<ISpendingRepository> spendingRepositoryMock = new Mock<ISpendingRepository>(MockBehavior.Strict);
            spendingRepositoryMock.Setup(r => r.ListByUser(It.IsAny<int>())).Returns(sourceSpendings);

            SpendingService sut = new SpendingService(spendingRepositoryMock.Object, null);
            IEnumerable<Spending> spendings = sut.ListOrdered(2, SpendingSortOrder.ByAmount);

            Assert.Equal(sourceSpendings.Count, spendings.Count());
            string spendingsAsString = string.Join(',', spendings.Select(s => s.Comment));
            Assert.Equal("1,2,3", spendingsAsString);
        }

        [Fact]
        public void ListOrdered_ByDate()
        {
            List<Spending> sourceSpendings = new List<Spending>()
            {
                new Spending(){DateInUtc=new DateTime(2021,02,01), Comment="3"},
                new Spending(){DateInUtc=new DateTime(2021,01,01),Comment="2"},
                new Spending(){DateInUtc=new DateTime(2019,12,31), Comment="1"}
            };
            Mock<ISpendingRepository> spendingRepositoryMock = new Mock<ISpendingRepository>(MockBehavior.Strict);
            spendingRepositoryMock.Setup(r => r.ListByUser(It.IsAny<int>())).Returns(sourceSpendings);

            SpendingService sut = new SpendingService(spendingRepositoryMock.Object, null);
            IEnumerable<Spending> spendings = sut.ListOrdered(3, SpendingSortOrder.ByDate);

            Assert.Equal(sourceSpendings.Count, spendings.Count());
            string spendingsAsString = string.Join(',', spendings.Select(s => s.Comment));
            Assert.Equal("1,2,3", spendingsAsString);
        }
    }
}
