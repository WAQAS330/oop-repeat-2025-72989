using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Domain;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerIdentifier { get; set; }

    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string AccountPassword { get; set; } = null!;
    
    public List<int>? RegisteredVehicleIds = new List<int>();
    public ICollection<Car>? RegisteredVehicles { get; set; } = new List<Car>();
}