using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Razor.Models;

public class MechanicDTO
{
    public int MechanicId { get; set; }


    public string FullName { get; set; } = null!;

    public string AccountPassword { get; set; } = null!;
    
    public string EmailAddress { get; set; } = null!;

    public ICollection<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
}
