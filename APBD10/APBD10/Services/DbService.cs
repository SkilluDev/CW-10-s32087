using APBD10.Data;
using APBD10.Data;
using APBD10.DTOs;
using APBD10.Exceptions;
using APBD10.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace APBD10.Services;

public interface IDbService
{
    Task<TripPagedGetDto> GetTripsDetailsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int tripId, ClientTripPostDTO clientTrip);
    Task RemoveClientAsync(int clientId);
}

public class DbService(S32087Context data) : IDbService
{
    public async Task<TripPagedGetDto> GetTripsDetailsAsync(int page, int pageSize)
    {
        //TODO zwroc liste posortowana malejaco po dacie rozpoczecia wycieczki
        // musi korzysstac z page i pageSize
        var result = new List<TripGetDTO>();

        var trips = await data.Trips
            .Include(t => t.IdCountries)
            .ToListAsync();

        foreach (var trip in trips)
        {
            result.Add(await TripToGetDto(trip));
        }
        
        result.Sort((t1, t2) =>
        {
            if (t1.DateFrom < t2.DateFrom) return -1;
            if (t1.DateFrom > t2.DateFrom) return 1;
            return 0;
        });
        var length = result.Count;
        var allPages = (int)Math.Ceiling((double)length / pageSize);
        var howMany = pageSize < length - (page - 1) * pageSize ? pageSize : length - (page - 1) * pageSize;
        result = result.Slice((page-1)*pageSize, howMany);
        return new TripPagedGetDto
        {
            Trips = result,
            PageNumber = page,
            PageSize = pageSize,
            AllPages = allPages
        };
    }

    public async Task AddClientToTripAsync(int tripId, ClientTripPostDTO clientTrip)
    {
        var trip = await data.Trips.FindAsync(tripId);

        if (trip == null)
        {
            throw new ArgumentException($"Trip with id: {tripId} does not exist");
        }

        if (trip.DateTo < DateTime.Now)
        {
            throw new TripInThePastException($"Trip with id: {tripId} already happened");
        }
        
        var client = await data.Clients.Where(c=>c.Pesel==clientTrip.Pesel).FirstOrDefaultAsync();
        
        if (client == null)
        {
            throw new ArgumentException($"Client with pesel: {clientTrip.Pesel} does not exist");
        }
        
        var clientsTrip = await data.ClientTrips.Where(ct=>ct.IdClient==client.IdClient && ct.IdTrip == tripId).ToListAsync();

        if (clientsTrip.Any())
        {
            throw new ClientInTripException($"Client with pesel: {clientTrip.Pesel} already is assigned to this trip");
        }

        var newClientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = tripId,
            PaymentDate = clientTrip.PaymentDate,
            RegisteredAt = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))
        };
        
        await data.ClientTrips.AddAsync(newClientTrip);
        await data.SaveChangesAsync();
    }

    public async Task RemoveClientAsync(int clientId)
    {
        var client  = data.Clients.FirstOrDefault(c=>c.IdClient == clientId);

        if (client == null)
        {
            throw new ArgumentException($"Client with id: {clientId} does not exist");
        }
        var clientTrips = data.ClientTrips.Where(ct=>ct.IdClient == clientId);
        if (clientTrips.Any())
        {
            throw new ClientHasTripsException($"Client with id: {clientId} has trips assigned");
        }
        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }

    public async Task<TripGetDTO> TripToGetDto(Trip trip)
    {
        var tripGetDto = new TripGetDTO
        {
            Id = trip.IdTrip,
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            CountryList = [],
            ClientList = [],
        };
        var countries = await data.Countries.Where(c => trip.IdCountries.Select(co => co.IdCountry).Contains(c.IdCountry)).ToListAsync();
        foreach (var country in countries)
        {
            tripGetDto.CountryList.Add(CountryToGetDto(country));
        }

        var clients = await data.ClientTrips.Where(ct=>ct.IdTrip==trip.IdTrip).Join(
                data.Clients, ct => ct.IdClient, c => c.IdClient, (ct, c) => c)
            .ToListAsync();
        foreach (var client in clients)
        {
            tripGetDto.ClientList.Add(ClientToGetDto(client));
        }
        
        return tripGetDto;
    }

    public CountryGetDTO CountryToGetDto(Country country)
    {
        return new CountryGetDTO()
        {
            Name = country.Name,
        };
    }
    
    public ClientGetDTO ClientToGetDto(Client client)
    {
        return new ClientGetDTO()
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
        };
    }
}