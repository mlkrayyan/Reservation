using Microsoft.AspNetCore.Mvc;
using SpaReservation.Application.Common.Models;
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
    public async Task<ActionResult<PagedResult<SalonService>>> GetActive(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 6,
        [FromQuery] ServiceCategory? category = null)
    {
        var (items, totalCount) = await _repository.GetPagedActiveAsync(pageNumber, pageSize, category);
        var result = PagedResult<SalonService>.Create(items.ToList(), pageNumber, pageSize, totalCount);
        return Ok(result);
    }
}
