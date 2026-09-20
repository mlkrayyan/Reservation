using FluentAssertions;
using SpaReservation.Domain.Customers;

namespace SpaReservation.Domain.Tests.Customers;

public class CustomerTests
{
    [Fact]
    public void AwardLoyaltyPoints_ShouldIncrementPointsAndAddTransaction()
    {
        var customer = Customer.Create("Test User", "test@test.com", "+9647001234567", "google_123", "Google");
        customer.AwardLoyaltyPoints(10, "Completed booking", Guid.NewGuid());
        customer.LoyaltyPoints.Should().Be(10);
        customer.LoyaltyTransactions.Should().HaveCount(1);
        customer.LoyaltyTransactions[0].PointsDelta.Should().Be(10);
    }

    [Fact]
    public void DeductLoyaltyPoints_WhenInsufficientPoints_ShouldThrow()
    {
        var customer = Customer.Create("Test User", null, null, "google_123", "Google");
        var act = () => customer.DeductLoyaltyPoints(50, "Redemption");
        act.Should().Throw<InvalidOperationException>().WithMessage("*Insufficient*");
    }

    [Fact]
    public void DeductLoyaltyPoints_WhenSufficientPoints_ShouldSucceed()
    {
        var customer = Customer.Create("Test User", null, null, "google_123", "Google");
        customer.AwardLoyaltyPoints(100, "Welcome bonus");
        customer.DeductLoyaltyPoints(50, "Redemption");
        customer.LoyaltyPoints.Should().Be(50);
        customer.LoyaltyTransactions.Should().HaveCount(2);
    }
}
