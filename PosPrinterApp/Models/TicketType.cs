using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class TicketType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }

    public string Title { get; set; }
    public int Price { get; set; } = 0;
    
    public override string ToString() => Title;
}