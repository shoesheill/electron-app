namespace TicketApp.Models;

using System.ComponentModel.DataAnnotations;

public class Theater
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string VatNo { get; set; }

    public bool IsVat { get; set; }
    public int ThreeDCharge { get; set; }

    [Range(0, 100)]
    public decimal FDF { get; set; }

    [Range(0, 100)]
    public decimal LocalTax { get; set; }
    [Range(0, 100)]
    public decimal LocalTaxInternational { get; set; }
    [Range(0, 100)]
    public decimal BoxOfficeTax { get; set; }
    [Range(0, 100)]
    public decimal EntertainmentTax { get; set; }
    public decimal ConvinienceCharge { get; set; }
    public bool IsCCMS { get; set; }
    public string? LogoPath { get; set; }
    public bool QRCode { get; set; }
    public bool EnableTaxRegistration { get; set; }
}
