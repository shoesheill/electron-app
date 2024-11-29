namespace TicketApp.Models;

public class TicketSeat
{

    public int Id { get; set; }
    public Ticket Ticket { get; set; }
    public int TicketId { get; set; }
    public int ScreenSeatId { get; set; }
    public ScreenSeat ScreenSeat { get; set; }
    //public TYPE TicketCode { get; set; }
}