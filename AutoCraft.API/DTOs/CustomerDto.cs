namespace AutoCraft.API.DTOs;

public class CustomerDto
{
    public int CustomerIdentifier { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public List<int> RegisteredVehicleIds { get; set; } = new List<int>();
} 