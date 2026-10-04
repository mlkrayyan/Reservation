using Microsoft.AspNetCore.Mvc;
using SpaReservation.Application.Common.Models;
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
    public async Task<ActionResult<PagedResult<StaffMember>>> GetActive(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 6)
    {
        var (items, totalCount) = await _repository.GetPagedActiveAsync(pageNumber, pageSize);
        var result = PagedResult<StaffMember>.Create(items.ToList(), pageNumber, pageSize, totalCount);
        return Ok(result);
    }
}
