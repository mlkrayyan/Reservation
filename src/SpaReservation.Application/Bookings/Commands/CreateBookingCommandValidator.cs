using FluentValidation;

namespace SpaReservation.Application.Bookings.Commands;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.Date).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date));
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}
