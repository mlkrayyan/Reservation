using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Customers;
public class Customer : AggregateRoot
{
    private Customer() { }
    public string FullName { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string PreferredLanguage { get; private set; } = "ar";
    public int LoyaltyPoints { get; private set; }
    public string? ExternalAuthId { get; private set; }
    public string? AuthProvider { get; private set; }
    private readonly List<LoyaltyTransaction> _loyaltyTransactions = new();
    public IReadOnlyList<LoyaltyTransaction> LoyaltyTransactions => _loyaltyTransactions.AsReadOnly();

    public static Customer Create(string fullName, string? email, string? phone, string externalAuthId, string authProvider)
        => new() { FullName = fullName, Email = email, Phone = phone, ExternalAuthId = externalAuthId, AuthProvider = authProvider };

    public void UpdateProfile(string fullName, string? phone, string preferredLanguage)
    { FullName = fullName; Phone = phone; PreferredLanguage = preferredLanguage; }

    public void AwardLoyaltyPoints(int points, string reason, Guid? bookingId = null)
    {
        LoyaltyPoints += points;
        _loyaltyTransactions.Add(LoyaltyTransaction.Create(Id, points, reason, bookingId));
    }

    public void DeductLoyaltyPoints(int points, string reason)
    {
        if (LoyaltyPoints < points) throw new InvalidOperationException("Insufficient loyalty points.");
        LoyaltyPoints -= points;
        _loyaltyTransactions.Add(LoyaltyTransaction.Create(Id, -points, reason));
    }
}
