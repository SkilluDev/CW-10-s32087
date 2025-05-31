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
    Task AddClientToTripAsync(int tripId);
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
            else if (t1.DateFrom > t2.DateFrom) return 1;
            else return 0;
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

    public async Task AddClientToTripAsync(int tripId)
    {
        //TODO jesli klient o takim peselu juz przypisany do wycieczki- blad
        // czy dana wycieczka nie istnieje albo jest w przeszlosci - blad
        // paymentdate moze byc null
        //registeredat to aktualny czas
        throw new NotImplementedException();
    }

    public async Task RemoveClientAsync(int clientId)
    {
        //TODO check if client exists;
        //check if client has any trips-> if yes than abort with message;
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
        Console.WriteLine("IC"+trip.IdCountries.Count);
        var countries = data.Countries.Where(c => trip.IdCountries.Select(co => co.IdCountry).Contains(c.IdCountry));
        foreach (var country in countries)
        {
            tripGetDto.CountryList.Add(CountryToGetDto(country));
        }

        var clients = data.ClientTrips.Where(ct=>ct.IdTrip==trip.IdTrip).Join(
                data.Clients, ct => ct.IdClient, c => c.IdClient, (ct, c) => c)
            .ToList();
        //Console.WriteLine(clients.Count);
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