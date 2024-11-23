using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosPrinterApp;

namespace PosPrinterApp.Helper
{
    public class PrintUtil
    {
        public void Print()
        {
            try
            {
                for (int i = 0; i < StaticData.TicketCount; i++)
                    Task.Factory.StartNew(() =>
                    {
                        StaticData sd = new StaticData();
                        DynamicPrinter objDynamicPrinter = new DynamicPrinter();
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
