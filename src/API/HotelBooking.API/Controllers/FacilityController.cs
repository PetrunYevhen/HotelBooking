using Facilities.Application.Command.AddNewFacility;
using Facilities.Application.Contracts;
using Facilities.Application.Query.GetAllFacilities;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("facility")]
public class FacilityController : ControllerBase
{
    private readonly IFacilityModule _facilityModule;

    public FacilityController(IFacilityModule facilityModule)
    {
        _facilityModule = facilityModule;
    }

    [HttpGet("get-all-facilities")]
    public async Task<IActionResult> GetAllFacilities()
    {
        var result = await _facilityModule.ExecuteQueryAsync(new GetAllFacilitiesQuery());
        return Ok(result);
    }
    
    [HttpPost("add-facility")]
    public async Task<IActionResult> AddFacility([FromBody] AddNewFacilityCommand command)
    {
        var result = await _facilityModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}