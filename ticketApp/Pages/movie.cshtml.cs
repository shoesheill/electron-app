using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketApp.Data;
using TicketApp.Models;

namespace ticketApp.Pages;

public class movie : PageModel
{
    private readonly ILogger<movie> _logger;
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public movie(AppDbContext context, ILogger<movie> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    public IList<Movie> Movies { get; set; }

    public async Task OnGetAsync()
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now); // Get the current date
        var currentTime = TimeOnly.FromDateTime(DateTime.Now); // Get Current Time

        Movies = await _context.Movies
            .AsNoTracking()
            .Include(movie => movie.Shows)
            .Where(movie => movie.Shows.Any(show => show.Date == currentDate && show.StartTime > currentTime))
            .ToListAsync();
    }

    public async Task<JsonResult> OnGetGetScreenLayoutWithSeatStatusAsync(int showId)
    {
        string soldSeatsKey = $"SoldSeats_{showId}";
        string screenIdKey = $"ScreenId_{showId}";
        var screenId = await _cache.GetOrCreateAsync(screenIdKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return _context.Shows
                .AsNoTracking()
                .Where(show => show.Id == showId)
                .Select(show => show.ScreenId)
                .FirstOrDefaultAsync();
        });
        string screenKey = $"Screen_{screenId}";
        string screenSeatsKey = $"ScreenSeats_{screenId}";
        // Step 1: Get screen details
        var screen = await _cache.GetOrCreateAsync(screenKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _context.Screen
                .Where(s => s.Id == screenId)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Row,
                    s.Column,
                    s.Seats
                })
                .FirstOrDefaultAsync();
        });

        if (screen == null)
        {
            throw new Exception("Screen not found.");
        }

        // Step 2: Get screen seats
        var screenSeats = await _cache.GetOrCreateAsync(screenSeatsKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _context.ScreenSeat
                .Where(ss => ss.ScreenId == screenId)
                .ToListAsync();
        });

        // Step 3: Get sold seat IDs for the specified show
        var soldSeatIds = await _cache.GetOrCreateAsync(soldSeatsKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return await _context.TicketSeat
                .Where(ts => ts.Ticket.ShowId == showId)
                .Select(ts => ts.ScreenSeatId)
                .ToListAsync();
        });

        // Step 4: Group seats by RowNo
        var seatsGroupedByRow = screenSeats
            .GroupBy(seat => seat.RowNo)
            .Select(group => new
            {
                RowNo = group.Key,
                Seats = group.Select(seat => new
                {
                    seat.Id,
                    seat.SeatNo,
                    seat.ColNo,
                    Status = soldSeatIds.Contains(seat.Id) ? "Sold" : "Available"
                }).ToList()
            })
            .ToList();

        // Step 5: Structure the result
        var result = new
        {
            Screen = screen,
            Rows = seatsGroupedByRow
        };

        return new JsonResult(result);
    }

    public async Task<JsonResult> OnGetPurchaseTicketAsync(string seatIds, int showId)
    {
        // Split the Seat IDs into a list of integers
        var selectedSeatIds = seatIds
            .Split(',')
            .Select(int.Parse)
            .ToList();
       int ticketPrice = await _context.Shows
            .AsNoTracking()
            .Include(show => show.TicketType)
            .Where(show => show.Id == showId)
            .Select(show => show.TicketType.Price)
            .FirstOrDefaultAsync();

        await using var transaction = await _context.Database.BeginTransactionAsync(); // Begin a transaction

        try
        {
            // Step 1: Insert into Transactions
            var newTransaction = new Transaction()
            {
                PurchaseDate = DateTime.Now,
                TotalAmount = ticketPrice * selectedSeatIds.Count // Calculate total price
            };

            _context.Transaction.Add(newTransaction);
            await _context.SaveChangesAsync(); // Commit the transaction for Transactions

            // Step 2: Insert into Tickets
            var newTicket = new Ticket()
            {
                TransactionId = newTransaction.Id,
                ShowId = showId,
                Price = ticketPrice
            };

            _context.Ticket.Add(newTicket);
            await _context.SaveChangesAsync(); // Commit the transaction for Tickets

            // Step 3: Insert into TicketSeats
            var ticketSeats = selectedSeatIds.Select(seatId => new TicketSeat()
            {
                TicketId = newTicket.Id,
                ScreenSeatId = seatId
            }).ToList();

            _context.TicketSeat.AddRange(ticketSeats);
            await _context.SaveChangesAsync(); // Commit the transaction for TicketSeats

            // If all operations are successful, commit the entire transaction
            await transaction.CommitAsync();

            return new JsonResult(new { id = newTicket.Id });
        }
        catch (Exception ex)
        {
            // If any error occurs, rollback the transaction
            Console.WriteLine($"An error occurred: {ex.Message}");
            await transaction.RollbackAsync(); // Rollback all changes

            // Handle the exception (log it, rethrow it, etc.)
        }

        return new JsonResult(new { id = 0 });
    }
}