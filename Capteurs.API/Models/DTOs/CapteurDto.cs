namespace Capteurs.API.Models.DTOs;

public class CapteurDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
