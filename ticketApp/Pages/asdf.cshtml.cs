using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;

namespace ticketApp.Pages;

public class Asdf : PageModel
{
    private readonly AppDbContext _context;
    public Asdf(AppDbContext context)
    {
        _context = context;
    }
    [BindProperty]
    public Theater Theater { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // Fetch the theater details for display
        Theater = await _context.Theater.AsNoTracking().FirstOrDefaultAsync();

        // if (Theater == null)
        // {
        //     return NotFound();
        // }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _context.Theater
            .Where(theater => theater.Id > 0)
            .ExecuteDeleteAsync();
      
       await _context.Theater.AddAsync(Theater);
       var result=await _context.SaveChangesAsync();

        if (result == 0)
        {
            return NotFound(); // No rows were affected, handle accordingly.
        }

        return RedirectToPage("Index");
    }
}