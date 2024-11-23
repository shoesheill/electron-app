using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class Show
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }

    public DateOnly Date { get; set; }=new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    public TimeOnly StartTime { get; set; }=new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
    [Display(AutoGenerateField = false)] 
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }
    public int ScreenId { get; set; }
    [ForeignKey("ScreenId")]
    public Screen Screen { get; set; }
}