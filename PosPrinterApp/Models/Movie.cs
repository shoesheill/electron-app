using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TicketApp.Models;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }

    public string Title { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsInternational { get; set; }
    public bool IsThreeD { get; set; }
    

    public override string ToString() => Title;
}