using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;

namespace ticketApp.Pages;

public class seatlayout : PageModel
{
    private readonly AppDbContext _context;

    public seatlayout(AppDbContext context)
    {
        _context = context;
    }
   public IList<Screen> Screens { get; set; }
   public IList<TicketType> TicketTypes { get; set; }
    public async Task OnGetAsync()
    {
         TicketTypes = await _context.TicketTypes
            .ToListAsync();
         Screens = await _context.Screen
            .ToListAsync();
        //await OnGetGenerateSeatLayoutAsync();
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