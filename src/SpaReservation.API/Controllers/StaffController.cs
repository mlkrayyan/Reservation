using Microsoft.AspNetCore.Mvc;
using SpaReservation.Domain.Staff;

namespace SpaReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffRepository _repository;

    public StaffController(IStaffRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffMember>>> GetActive()
    {
        var staff = await _repository.GetActiveAsync();
        return Ok(staff);
    }
}
