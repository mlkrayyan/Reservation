using MediatR;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Domain.SharedKernel;

namespace SpaReservation.Application.Bookings.Commands;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var slot = new BookingSlot(request.Date, request.StartTime, request.EndTime);
        
        // Basic validation...
        var booking = Booking.Create(request.CustomerId, request.StaffId, request.ServiceId, slot, request.Notes);
        
        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return booking.Id;
    }
}
