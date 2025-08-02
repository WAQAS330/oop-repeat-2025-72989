using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Domain;

public class Mechanic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MechanicId { get; set; }

    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string AccountPassword { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;

    public ICollection<Service> Services { get; set; } = new List<Service>();
}
