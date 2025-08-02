using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Domain;

public class Service
{
 
    public int ServiceId { get; set; }

    public int VehicleId { get; set; }
    public Car Vehicle { get; set; } = null!;

    public int MechanicId { get; set; }
    public Mechanic Mechanic { get; set; } = null!;
   


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
        int hoursRounded = (int)Math.Ceiling(Hours);
        TotalCost = hoursRounded * 75;
    }
}