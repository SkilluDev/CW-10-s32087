using System.ComponentModel.DataAnnotations;

namespace APBD10.DTOs;

public class ClientTripPostDTO
{
    [MaxLength(120)]
    public string FirstName { get; set; }
    
    [MaxLength(120)]
    public string LastName { get; set; }
    
    [MaxLength(120)]
    public string Email { get; set; }
    
    [MaxLength(120)]
    public string Telephone { get; set; }
    
    [MaxLength(120)]
    public string Pesel { get; set; }
    
    public int IdTrip {get; set;}
    
    [MaxLength(120)]
    public string TripName {get; set;}
    
    public int? PaymentDate {get; set;}
}