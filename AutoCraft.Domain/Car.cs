using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Domain;

public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VehicleId { get; set; }

    [Required]
    public string LicensePlateNumber { get; set; } = null!;

    public int VehicleOwnerId { get; set; }
    public Customer? VehicleOwner { get; set; } = null!;

    public ICollection<Service>? MaintenanceServices { get; set; } = new List<Service>();
}
