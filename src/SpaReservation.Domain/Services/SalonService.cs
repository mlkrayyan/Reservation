using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Services;
public class SalonService : AggregateRoot
{
    private SalonService() { }
    public string NameAr { get; private set; } = string.Empty;
    public string NameEn { get; private set; } = string.Empty;
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }
    public ServiceCategory Category { get; private set; }
    public int DurationMinutes { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    public static SalonService Create(string nameAr, string nameEn, ServiceCategory category, int durationMinutes, decimal price, string? descAr = null, string? descEn = null, string? imageUrl = null)
    {
        return new SalonService { NameAr = nameAr, NameEn = nameEn, DescriptionAr = descAr, DescriptionEn = descEn, Category = category, DurationMinutes = durationMinutes, Price = price, ImageUrl = imageUrl };
    }

    public void Update(string nameAr, string nameEn, ServiceCategory category, int durationMinutes, decimal price, string? descAr, string? descEn, string? imageUrl)
    {
        NameAr = nameAr; NameEn = nameEn; Category = category; DurationMinutes = durationMinutes;
        Price = price; DescriptionAr = descAr; DescriptionEn = descEn; ImageUrl = imageUrl;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
