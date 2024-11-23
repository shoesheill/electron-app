using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class TicketPrice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }

    public int ShowId { get; set; }
    [ForeignKey("ShowId")] 
    public Show Show { get; set; }

    public int TicketTypeId { get; set; }
    [ForeignKey("TicketTypeId")]
    public TicketType TicketType { get; set; }
}