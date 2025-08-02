using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoCraft.Domain;

namespace AutoCraft.Razor.Models;

public class ServiceDTO
{

    public int ServiceId { get; set; }

    public int VehicleId { get; set; }
    public CarDTO Vehicle { get; set; } = null!;

    public string LicensePlateNumber { get; set; } = string.Empty;

    public int MechanicId { get; set; }
    public MechanicDTO Mechanic { get; set; } = null!;
    public String MechanicName { get; set; }


    [Required]
    public DateTime ServiceDate { get; set; }

    public string? WorkDescription { get; set; }

    [Range(0, int.MaxValue)]
    public decimal Hours { get; set; }

    public DateTime? CompletionDate { get; set; }

    public decimal? TotalCost { get; set; }

    public string Status { get; set; } = "Pending";



    public void CalculateCost()
    {
        // Round up hours to next integer
        int hoursRounded = (int)Math.Ceiling(Hours);
        TotalCost = hoursRounded * 75;
    }
}