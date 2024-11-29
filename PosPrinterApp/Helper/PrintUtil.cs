using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosPrinterApp;
using TicketApp.Models;
using PosPrinterApp.DTO;

namespace PosPrinterApp.Helper
{
    public class PrintUtil
    {

        public void Print(IList<TicketDto> tickets, Theater theater)
        {
            try
            {
               // for (int i = 0; i < StaticData.TicketCount; i++)
               foreach (TicketDto seat in tickets)
                    Task.Factory.StartNew(() =>
                    {
                        //StaticData sd = new StaticData();
                        DynamicPrinter objDynamicPrinter = new DynamicPrinter();
                        objDynamicPrinter.ticketDetail = seat;
                        objDynamicPrinter.theater = theater;
                        objDynamicPrinter.Print(PrintType.Default);

                    }).Wait();
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error while printing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        
    }
}
