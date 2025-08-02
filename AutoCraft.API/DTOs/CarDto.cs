namespace AutoCraft.API.DTOs;

public class CarDto
{
    public int VehicleId { get; set; }
    public string LicensePlateNumber { get; set; } = string.Empty;
    public int VehicleOwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
} 