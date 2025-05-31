using APBD10.Exceptions;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Models.DTOs;

namespace TravelAgency.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IDbService dbService) : ControllerBase
{

    //Delete the client from a certain trip by id
    [HttpDelete]
    [Route("{clientId}")]
    public async Task<IActionResult> RemoveClient([FromRoute] int clientId)
    {
        try
        {
            await dbService.RemoveClientAsync(clientId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }
}