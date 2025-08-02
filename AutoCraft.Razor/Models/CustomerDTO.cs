using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Razor.Models;

public class CustomerDTO
{
    public int CustomerIdentifier { get; set; }
    
    public string FullName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string AccountPassword { get; set; } = null!;
    public List<int>? RegisteredVehicleIds = new List<int>();
    public ICollection<CarDTO>? RegisteredVehicles { get; set; } = new List<CarDTO>();
}