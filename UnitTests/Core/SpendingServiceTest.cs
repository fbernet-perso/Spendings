using System;
using System.Globalization;
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
        [InlineData(1, 12, 100, "USD", Nature.Misc, "Taxi", SpendingCreationError.SpendingDateInTheFuture)]
        [InlineData(1, -12, 100, "USD", Nature.Misc, "Taxi", SpendingCreationError.SpendingDateExpired)]
        [InlineData(1, -1, 100, "USD", Nature.Misc, "", SpendingCreationError.MissingOrEmptyComment)]
        [InlineData(1, -1, 100, "RUB", Nature.Misc, "Taxi", SpendingCreationError.SpendingCurrencyDiscrepancy)]
        [InlineData(1, -1, -100, "USD", Nature.Misc, "Taxi", SpendingCreationError.SpendingAmountBelow0)]
        [InlineData(3, -1, 100, "USD", Nature.Misc, "Taxi", SpendingCreationError.UserNotFound)]
        [InlineData(1, -1, 42, "USD", Nature.Misc, "Taxi", SpendingCreationError.DuplicateSpending)]
        [InlineData(1, 12, 100, "RUB", Nature.Misc, "", SpendingCreationError.MissingOrEmptyComment | SpendingCreationError.SpendingCurrencyDiscrepancy | SpendingCreationError.SpendingDateInTheFuture)]
        [InlineData(1, -1, 100, "USD", Nature.Misc, "Taxi", SpendingCreationError.None)]
        public void AddSpending(int userId, int monthToAdd, decimal amount, string currency, Nature nature, string comment, SpendingCreationError expectedOutcome)
        {
            // Arrange
            User ironMan = new User("Stark", "Anthony", new RegionInfo("US").ISOCurrencySymbol);
            User userNotFound = null;

            Mock<ISpendingRepository> spendingRepositoryMock = new Mock<ISpendingRepository>(MockBehavior.Strict);
            spendingRepositoryMock.Setup(r => r.AddSpending(It.IsAny<Spending>()));
            spendingRepositoryMock.Setup(r => r.LoadUserById(1)).Returns(ironMan);
            spendingRepositoryMock.Setup(r => r.LoadUserById(3)).Returns(userNotFound);

            spendingRepositoryMock.Setup(r => r.SpendingExists(It.IsAny<int>(), It.IsAny<DateTime>(), 42)).Returns(true);
            spendingRepositoryMock.Setup(r => r.SpendingExists(It.IsAny<int>(), It.IsAny<DateTime>(), 100)).Returns(false);
            spendingRepositoryMock.Setup(r => r.SpendingExists(It.IsAny<int>(), It.IsAny<DateTime>(), -100)).Returns(false);


            // Act
            // Adding/removing month to today
            DateTime spendingDate = DateTime.Today.AddMonths(monthToAdd);
            ISpendingService sut = new SpendingService(spendingRepositoryMock.Object);
            SpendingCreationError outcome = sut.TryCreateSpending(userId, spendingDate, amount, currency, nature, comment);

            // Assert
            Assert.Equal(expectedOutcome, outcome);
        }
    }
}
