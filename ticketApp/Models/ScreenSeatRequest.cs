namespace TicketApp.Models;

public class ScreenSeatRequest
{
    public int ScreenId { get; set; }           // Screen ID received from the client
    public List<ScreenSeat> ScreenSeats { get; set; }  // List of screen seats received from the client
}