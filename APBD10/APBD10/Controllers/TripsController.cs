using APBD10.Exceptions;
using APBD10.Models;
using APBD10.Services;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD10.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController(IDbService dbService):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTrips([FromQuery] int page =1,[FromQuery] int pageSize=10)
    {
        return Ok(await dbService.GetTripsDetailsAsync(page, pageSize));
    }
    
    [HttpPut]
    [Route("{tripId:int}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int tripId)
    {
        try
        {
            await dbService.AddClientToTripAsync(tripId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }
}