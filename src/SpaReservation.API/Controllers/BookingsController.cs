using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaReservation.Application.Bookings.Commands;
using SpaReservation.Application.Bookings.Queries;

namespace SpaReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBookingCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> Get(Guid id)
    {
        var result = await _mediator.Send(new GetBookingQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id}/confirm")]
    public async Task<ActionResult> Confirm(Guid id)
    {
        await _mediator.Send(new ConfirmBookingCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        await _mediator.Send(new CancelBookingCommand(id, reason));
        return NoContent();
    }
}
