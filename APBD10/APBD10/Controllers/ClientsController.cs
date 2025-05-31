using APBD10.Exceptions;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD10.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IDbService dbService) : ControllerBase
{

    //Delete the client from a certain trip by id
    [HttpDelete]
    [Route("{clientId:int}")]
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