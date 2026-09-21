using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Domain.Customers;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Seed Services if empty
        if (!await context.Services.AnyAsync())
        {
            var services = new List<SalonService>
            {
                SalonService.Create("جلسة تدليك سويدي فاخرة", "Royal Swedish Massage", ServiceCategory.MassageTherapy, 60, 80, "علاج مهدئ للأعصاب واستلاخاء العضلات بالزيوت العطرية.", "Deep muscle relaxation treatment using essential lavender oils."),
                SalonService.Create("تنظيف بشرة هيدرافيشل", "Hydra-Facial Glow", ServiceCategory.FacialSkincare, 45, 65, "تنظيف وتقشير وترطيب عميق للبشرة لإشراقة طبيعية.", "Deep cleansing, exfoliation, and hydration for radiant skin."),
                SalonService.Create("عناية بالأظافر ومانيكير", "Luxury Spa Manicure & Pedicure", ServiceCategory.NailCare, 50, 50, "قص وتشكيل وتقشير الأظافر مع طلاء فاخر.", "Nail shaping, cuticle care, scrub, and luxury polish."),
                SalonService.Create("حمام زيت وتصفيف شعر", "Silk Hair Spa & Blowdry", ServiceCategory.HaircutStyling, 40, 45, "ماسك مغذي للشعر مع استشوار وتصفيف محترف.", "Nourishing hair mask treatment followed by professional styling."),
                SalonService.Create("تقشير وترطيب الجسم بالكامل", "Full Body Scrub & Wrap", ServiceCategory.BodyTreatments, 75, 110, "تقشير بملح البحر الميت مع تغليف بالطين المغذي.", "Dead Sea salt exfoliation with detoxifying clay wrap."),
                SalonService.Create("تركيب رموش كلاسيك", "Eyelash Extensions Classic", ServiceCategory.EyelashEyebrow, 90, 95, "تركيب رموش خفيفة وفاخرة لإطلالة طبيعية.", "Full set of premium lightweight eyelash extensions.")
            };

            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();
        }

        // 2. Seed Staff Members if empty
        if (!await context.Staff.AnyAsync())
        {
            var staff1 = StaffMember.Create("Dr. Fatima Al-Zahra", "Facial & Skincare Specialist with 10+ years experience", null);
            var staff2 = StaffMember.Create("Amina Khalil", "Lead Massage Therapist certified in Swedish & Deep Tissue", null);
            var staff3 = StaffMember.Create("Layla Hassan", "Senior Hair Stylist and keratherapies expert", null);
            var staff4 = StaffMember.Create("Zainab Omar", "Nail Artist & Esthetician", null);

            // Add Working Schedules (Sunday to Thursday 9 AM to 5 PM)
            foreach (var staff in new[] { staff1, staff2, staff3, staff4 })
            {
                for (int day = 0; day <= 6; day++)
                {
                    var dayOfWeek = (DayOfWeek)day;
                    bool isOff = dayOfWeek == DayOfWeek.Friday;
                    staff.AddWorkingSchedule(WorkingSchedule.Create(staff.Id, dayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0), isOff));
                }
            }

            await context.Staff.AddRangeAsync(staff1, staff2, staff3, staff4);
            await context.SaveChangesAsync();
        }

        // 3. Seed Sample Customer if empty
        if (!await context.Customers.AnyAsync())
        {
            var customer = Customer.Create("Sara Ahmed", "sara@example.com", "+9647701234567", "c-101", "EmailPassword");
            customer.AwardLoyaltyPoints(50, "Welcome Bonus Reward");

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Seed Sample Booking
            var service = await context.Services.FirstOrDefaultAsync();
            var staffMember = await context.Staff.FirstOrDefaultAsync();

            if (service != null && staffMember != null)
            {
                var slot = new BookingSlot(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)), new TimeOnly(10, 0), new TimeOnly(11, 0));
                var booking = SpaReservation.Domain.Bookings.Booking.Create(customer.Id, staffMember.Id, service.Id, slot, "Special request: extra lavender oil");

                booking.Confirm();


                await context.Bookings.AddAsync(booking);
                await context.SaveChangesAsync();
            }
        }
    }
}
