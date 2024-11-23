using Microsoft.EntityFrameworkCore;
using TicketApp.Models;

namespace TicketApp.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<Screen> Screen { get; set; }
    public DbSet<ScreenSeat> ScreenSeat { get; set; }
    public DbSet<TicketSeat> TicketSeat { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Theater> Theater { get; set; }
}