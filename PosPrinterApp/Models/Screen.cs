using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TicketApp.Models;

public class Screen
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }
    public string Title { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int Seats { get; set; }
    // public displayorder Type { get; set; }
    // public rowdisplayorder Type { get; set; }
    // public colplayorder Type { get; set; }
    // public screenposition Type { get; set; }
    // [JsonIgnore]
    // public ICollection<Show> Shows { get; set; }
    //
    // // [JsonIgnore] 
    // // public ICollection<Rows> Rows{ get; set; }
    //[JsonIgnore]
    //public ICollection<ScreenSeats> ScreenSeats { get; set; }

    public override string ToString() => Title;
}