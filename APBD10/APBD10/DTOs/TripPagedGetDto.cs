namespace APBD10.DTOs;

public class TripPagedGetDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripGetDTO> Trips { get; set; }
}