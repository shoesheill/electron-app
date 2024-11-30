// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using PosPrinterApp.Data;
using PosPrinterApp.DTO;
using PosPrinterApp.Helper;
using TicketApp.Models;

namespace PosPrinterApp;

internal class Program
{
    private static void Main(string[] args)
    {
        //args = ["print://ticket?id=27&name=John"];
        string folderPath = @"C:\System32";
        string connetionString = $"{folderPath}\\ticketapp.db";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        if (args == null || args.Length == 0)
            return;
        var parsedData = ParseUrl(args[0]);
        if (parsedData.TryGetValue("scheme", out string scheme) && !string.IsNullOrEmpty(scheme) && scheme != "print")
            return;
        var optionsBuilder = new DbContextOptionsBuilder<PrinterDbContext>();
        optionsBuilder.UseSqlite(
            $"Data Source=C:\\System32\\ticketapp.db");


        parsedData.TryGetValue("id", out string ticketId);
        if (parsedData.TryGetValue("path", out string path) && !string.IsNullOrEmpty(path) && path == "ticket")
        {
            //using (var _context = new PrinterDbContext(optionsBuilder.Options))
            //{
            //    _context.Database.EnsureCreated();
            //    var data = _context.TicketTypes.ToList();
            //}
            PrintTicket(optionsBuilder, Convert.ToInt32(ticketId));
            // new ReceiptPrint().Print("XP-80", args[0].Replace("print://", string.Empty).Replace("/", string.Empty));
            //new ReceiptPrint().Print("asd", "print://1234".Replace("print://", string.Empty).Replace("/", string.Empty));

        }
    }

    private static async void PrintTicket(DbContextOptionsBuilder<PrinterDbContext> options, int ticketId)
    {
        using (var _context = new PrinterDbContext(options.Options))
        {
            _context.Database.EnsureCreated();
            var tickets = await _context.TicketSeat
     .AsNoTracking()
     .Where(ticketSeat => ticketSeat.TicketId == ticketId)
     .Select(ticketSeat => new TicketDto
     (
         ticketSeat.ScreenSeat.SeatNo ?? "",
         ticketSeat.Ticket.Show.Movie.Title,
         ticketSeat.Ticket.Show.Date,
         ticketSeat.Ticket.Show.StartTime,
         ticketSeat.Ticket.Show.Screen.Title,
         ticketSeat.ScreenSeat.TicketType.Title,
         ticketSeat.ScreenSeat.TicketType.Price,
         ticketSeat.Ticket.Show.Movie.IsThreeD,
         ticketSeat.Ticket.Show.Movie.IsInternational
     ))
     .ToListAsync();

            if (tickets.Count > 0)
            {
                string theaterKey = "Theater";
                var theater = await _context.Theater
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (theater != null)
                {
                    StaticData.IsCCMS=theater.IsCCMS;
                    new PrintUtil().Print(tickets, theater);
                }
            }
        }
    }
    static void LogToFile(string message)
    {
        string filePath = "c:\\System32\\log.txt";
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

        // Append the log message to the file
        File.AppendAllText(filePath, logMessage + Environment.NewLine);
    }
    static Dictionary<string, string> ParseUrl(string url)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            // Split the scheme from the rest of the URL
            var schemeSplit = url.Split(new[] { "://" }, 2, StringSplitOptions.None);
            if (schemeSplit.Length == 2)
            {
                result["scheme"] = schemeSplit[0];
                url = schemeSplit[1];
            }

            // Split the path and query string
            var pathSplit = url.Split(new[] { '?' }, 2, StringSplitOptions.None);
            string path = pathSplit[0];

            // Remove trailing slash if it exists
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }

            result["path"] = path;

            if (pathSplit.Length == 2)
            {
                var queryString = pathSplit[1];
                var queryParams = queryString.Split('&');
                foreach (var param in queryParams)
                {
                    var kvp = param.Split('=', 2);
                    if (kvp.Length == 2)
                    {
                        result[kvp[0]] = kvp[1];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing URL: {ex.Message}");
        }

        return result;
    }

}