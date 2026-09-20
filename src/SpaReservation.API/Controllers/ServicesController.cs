using Microsoft.AspNetCore.Mvc;
using SpaReservation.Domain.Services;

namespace SpaReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISalonServiceRepository _repository;

    public ServicesController(ISalonServiceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalonService>>> GetActive()
    {
        var services = await _repository.GetActiveAsync();
        return Ok(services);
    }
}
