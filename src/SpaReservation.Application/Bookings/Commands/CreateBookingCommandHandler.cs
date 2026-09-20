using MediatR;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Application.Bookings.Commands;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateBookingCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var slot = new BookingSlot(request.Date, request.StartTime, request.EndTime);
        
        // Basic validation...
        var booking = Booking.Create(request.CustomerId, request.StaffId, request.ServiceId, slot, request.Notes);
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        
        return booking.Id;
    }
}
