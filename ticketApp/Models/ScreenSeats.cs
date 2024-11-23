using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class ScreenSeat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public int Id { get; set; }
    public int ScreenId { get; set; }
    [ForeignKey("ScreenId")]
    public Screen Screen { get; set; }
    public int RowNo { get; set; }
    public int ColNo { get; set; }
    // public int RowId { get; set; }
    // public Rows Row { get; set; }
    // public int ColumnId { get; set; }
    // public Column Column { get; set; }
    // public seatrow Type { get; set; }
    // public customseatrow Type { get; set; }
    // public colname Type { get; set; }
    public bool? IsCustom { get; set; }
    public string? SeatNo { get; set; }
    public TicketType TicketType { get; set; }
    public int TicketTypeId { get; set; }
}