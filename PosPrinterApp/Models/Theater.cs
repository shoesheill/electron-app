using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosPrinterApp
{
    public class Theater
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string VatNo { get; set; }
        public bool IsVat { get; set; }
        public int ThreeDCharge { get; set; }
        public decimal FDF { get; set; }
        public decimal LocalTaxInternational { get; set; }
        public decimal LocalTax { get; set; }
        public decimal BoxOfficeTax { get; set; }
        public decimal EntertainmentTax { get; set; }
        public decimal ConvinienceCharge { get; set; }
        public bool IsCCMS { get; set; }
        public string? LogoPath { get; set; }
        public bool QRCode { get; set; }
        public bool EnableTaxRegistration { get; set; }

    }
}
