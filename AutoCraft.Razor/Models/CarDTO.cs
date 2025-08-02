using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCraft.Razor.Models
{
    public class CarDTO
    {
        
        public int VehicleId { get; set; }

        
        public string LicensePlateNumber { get; set; } = null!;

        public int VehicleOwnerId { get; set; }
        public CustomerDTO? VehicleOwner { get; set; } = null!;

        // Navigation property
        public ICollection<ServiceDTO>? MaintenanceServices { get; set; } = new List<ServiceDTO>();
    }
}
