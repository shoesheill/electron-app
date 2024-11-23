// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using PosPrinterApp.Data;
using PosPrinterApp.Helper;

namespace PosPrinterApp;

internal class Program
{
    private static void Main(string[] args)
    {
        args = ["print://ticket?id=1&name=John"];
        string folderPath = @"c:\SystemFiles";
        string connetionString =@"/Users/shoesheill/projects/dotnet/BlazorApp/WebApplication5/TicketApp/app.db";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        if (args == null || args.Length == 0)
            return;
        var parsedData = ParseUrl(args[0]);
        if (parsedData.TryGetValue("scheme", out string scheme) && !string.IsNullOrEmpty(scheme) && scheme!="print")
            return;
        var optionsBuilder = new DbContextOptionsBuilder<PrinterDbContext>();
        optionsBuilder.UseSqlite(
            "Data Source=/Users/shoesheill/projects/dotnet/ticket/ticketApp/app.db");


        parsedData.TryGetValue("id", out string ticketId);
        if (parsedData.TryGetValue("path", out string path) && !string.IsNullOrEmpty(path) && path == "ticket")
            PrintTicket(optionsBuilder,Convert.ToInt32(ticketId));
        // new ReceiptPrint().Print("XP-80", args[0].Replace("print://", string.Empty).Replace("/", string.Empty));
        //new ReceiptPrint().Print("asd", "print://1234".Replace("print://", string.Empty).Replace("/", string.Empty));
       
    }

    private static void PrintTicket(DbContextOptionsBuilder<PrinterDbContext> options,int ticketId)
    {
        using (var _context = new PrinterDbContext(options.Options))
        {

            var movies = _context.TicketSeat
                .AsNoTracking()
                .Where(ticket=>ticket.TicketId == ticketId)
                .ToList();
            //StaticData.TicketCount=movies.Count();
            //StaticData.lstTicket=movies.Select(movie => movie.Title).ToList();
            new PrintUtil().Print();
        }
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
            result["path"] = pathSplit[0];

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