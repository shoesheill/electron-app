using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class Ticket
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Display(AutoGenerateField = false)]
    public int Id { get; set; }
    public Show Show { get; set; }
    public int ShowId { get; set; }
    public int Price { get; set; }
    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    //public Status Status { get; set; } //(e.g., "Purchased", "CheckedIn", "Cancelled","Partial Checkedin", "Partial Cancelled")
    public DateTime? PurchaseDate { get; set; }
    //public TYPE TicketCode { get; set; }
}