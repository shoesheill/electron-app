using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketApp.Data;
using TicketApp.Models;

namespace ticketApp.Pages;

public class seatlayoutModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IOutputCacheStore _outputCache;

    public seatlayoutModel(AppDbContext context,IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }
   public IList<Screen> Screens { get; set; }
   public IList<TicketType> TicketTypes { get; set; }
   public IList<ScreenSeat> TicketSeats { get; set; }
   public int screenId;
   
   [BindProperty]
   public int ScreenId { get; set; }
   [BindProperty]
   public string Name { get; set; }
    
   [BindProperty]
   public List<ScreenSeat> ScreenSeats { get; set; } = new();

  public async Task<JsonResult> OnPostAsync()
{
    var form = Request.Form;
    int screenId = int.Parse(form["screenId"]);
    if(screenId == 0||ScreenId==null)
        return new JsonResult("");
    var password = form["password"];
    if(password!="github")
        return new JsonResult("");

    var screen =await _context.Screen.FirstOrDefaultAsync(x => x.Id == screenId);
    // Iterate through each row in the form
    foreach (var key in form.Keys)
    {
        if (key.StartsWith("rowText_"))
        {
            string row = key.Split('_')[1]; // Extract row number from "rowText_{row}"
            string rowName = form[key];

            // Get ticket type for this row
            string ticketTypeKey = $"ticketType_{row}";
            int ticketTypeId = int.Parse(form[ticketTypeKey]);

            // Find all seats for this row by looking for seat keys starting with "seat_{row}_"
            var seatsForRow = form.Keys
                .Where(k => k.StartsWith($"seat_{row}_"))
                .Select(k => new
                {
                    ColNo = int.Parse(k.Split('_')[2]), // Extract column number from the seat key
                    SeatNo = form[k]  // The seat number (name) if checked
                }).ToList();

            // Use the maximum column value found in the seat keys
            

            // Iterate through all columns for this row (up to the max column number found)
            for (int col = 1; col <= screen.Column; col++)
            {
                // Check if the current column seat exists in the form (i.e., it was selected)
                var seat = seatsForRow.FirstOrDefault(s => s.ColNo == col);

                // Create a new ScreenSeat for both checked and unchecked seats
                var screenSeat = new ScreenSeat
                {
                    RowNo = int.Parse(row),
                    RowName = rowName,
                    IsActive = seat != null,  // If seat is found, it's checked; otherwise, it's unchecked
                    ColNo = col,
                    SeatNo = seat?.SeatNo??"",   // If checked, use the seat number; otherwise, it's null (or empty string)
                    TicketTypeId = ticketTypeId,
                    ScreenId = screen.Id
                };

                // Add the seat to the list of ScreenSeats
                ScreenSeats.Add(screenSeat);
            }
        }
    }
    await _context.ScreenSeat
    .Where(ss => ss.ScreenId == screenId)
    .ExecuteDeleteAsync();
    await _context.ScreenSeat.AddRangeAsync(ScreenSeats);
    var savedItem=await _context.SaveChangesAsync();
    return new JsonResult(new { data = savedItem });
}



    public async Task OnGetAsync()
    {
         TicketTypes = await _context.TicketTypes
            .ToListAsync();
         Screens = await _context.Screen
            .ToListAsync();
        //await OnGetGenerateSeatLayoutAsync();
    }
    public async Task<JsonResult> OnGetScreenLayoutAsync(int screenId)
    {
        string screenLayoutKey = $"screenLayout_{screenId}";
        var  screenLayout = await _cache.GetOrCreateAsync(screenLayoutKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

            return  _context.Screen
            .Include(screen=>screen.ScreenSeats)
            .Where(screen=>screen.Id == screenId)
            .ToListAsync();
        });
        //TicketSeats = screenLayout??new List<ScreenSeat>();
        return new JsonResult(screenLayout);

    }

    public async Task<JsonResult> OnGetScreenSeatLayoutAsync(int screenId)
    {
        var screen= await  _context.Screen
        .Include(screen=>screen.ScreenSeats)
        .Where(screen=>screen.Id == screenId)
        .ToListAsync();
        var screenSeat = await _context.ScreenSeat
        .Where(screenSeat => screenSeat.ScreenId == screenId)
        .ToListAsync();
        var seatsGroupedByRow = screenSeat
        .GroupBy(seat => new { seat.RowNo, seat.RowName,seat.TicketTypeId })  // Group by both RowNo and RowName
        .Select(group => new
        {
            RowNo = group.Key.RowNo,      // Access RowNo from the key
            RowName = group.Key.RowName, 
            TicketTypeId =group.Key.TicketTypeId, // Access RowName from the key
            Seats = group.Select(seat => new
            {
                seat.Id,
                seat.SeatNo,
                seat.ColNo,
                seat.IsActive,
                seat.TicketTypeId
            }).ToList()
        })
        .ToList();

        return new JsonResult( new {screen=screen,seats=seatsGroupedByRow});
    }
    public async Task<JsonResult> OnGetGenerateSeatLayoutAsync(int ticketTypeId,int screenId,string password)
    {
        var screen=await _context.Screen
            .AsNoTracking()
            .Where(screen => screen.Id == screenId)
            .FirstOrDefaultAsync();
        if (screen.Row!>0 && screen.Column!>0 && password!="github")
            return new JsonResult(new{result="Invalid row column or password"});
        var entity = await _context.ScreenSeat
            .Where(seat => seat.ScreenId == screenId)
            .ToListAsync();
        if (entity != null)
        {
            _context.ScreenSeat.RemoveRange(entity);
           await _context.SaveChangesAsync();
        }
       var seats=await GenerateMatrixLabels(screen.Row,screen.Column,screenId,ticketTypeId);
       await _context.AddRangeAsync(seats);
       var result=await _context.SaveChangesAsync();
       return new JsonResult(new{result=result});
    }
    private static string GenerateSerial(int number)
    {
        string result = string.Empty;
        
        while (number > 0)
        {
            number--; // Decrease by 1 to make it 0-indexed
            result = (char)(number % 26 + 'A') + result; // Get character and prepend it to result
            number /= 26; // Move to the next digit in the serial
        }
        
        return result;
    }

    // Method to generate row-column matrix labels
    private async Task<List<ScreenSeat>> GenerateMatrixLabels(int rows, int cols,int screenId,int ticketType)
    {
        var seats = new List<ScreenSeat>();
        for (int row = 1; row <= rows; row++)
        {
            string rowLabel = GenerateSerial(row);  // Get the letter for the row (A, B, C, ...)
            
            for (int col = 1; col <= cols; col++)
            {
              seats.Add(new ScreenSeat
              {
                  RowNo = row,
                  ColNo = col,
                  SeatNo = rowLabel+col,
                  TicketTypeId = ticketType,
                  IsCustom = false,
                  ScreenId = screenId
              });
            }
        }
        return seats;
    }
}