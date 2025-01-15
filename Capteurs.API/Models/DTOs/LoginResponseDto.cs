using Capteurs.API.Models.Domain;

namespace Capteurs.API.Models.DTOs;

public class LoginResponseDto
{
    public UserDto User { get; set; }   
    public string Token { get; set; }
    public string Role { get; set; }
}
