using Microsoft.EntityFrameworkCore;
using TicketApp.Models;

namespace PosPrinterApp.Data;

public class PrinterDbContext:DbContext
{
    public PrinterDbContext(DbContextOptions<PrinterDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<Screen> Screen { get; set; }
    public DbSet<ScreenSeat> ScreenSeat { get; set; }
    public DbSet<TicketSeat> TicketSeat { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    //Disable Change Tracker
    public override int SaveChanges()=>throw new System.NotImplementedException();
}