using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosPrinterApp.DTO
{
    public record TicketDto
    (
        string SeatNo,
        string Movie,
        DateOnly ShowDate,
        TimeOnly StartTime,
        string Screen,
        string TicketType,
        int Price,
        bool IsThreeD,
        bool IsInternational
    );
}
