namespace TicketApp.Models;

public class Transaction
{
    public int Id { get; set; }
    //public UserId Type { get; set; }
    public int TotalAmount { get; set; }
    public DateTime? PurchaseDate { get; set; }
    //public status Type { get; set; } // active, cancelled, partially refund
}