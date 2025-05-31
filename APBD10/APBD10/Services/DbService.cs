using APBD10.Data;
using APBD10.Data;
using APBD10.Exceptions;
using APBD10.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models.DTOs;

namespace APBD10.Services;

public interface IDbService
{
    Task<IEnumerable<TripGetDTO>> GetTripsDetailsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int tripId);
    Task RemoveClientAsync(int clientId);
}

public class DbService(S32087Context data) : IDbService
{
    public async Task<IEnumerable<TripGetDTO>> GetTripsDetailsAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task AddClientToTripAsync(int tripId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveClientAsync(int clientId)
    {
        throw new NotImplementedException();
    }
}