using MediatR;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.SharedKernel;

namespace SpaReservation.Application.Bookings.Commands;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null) throw new Exception("Booking not found.");
        
        booking.Confirm();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
