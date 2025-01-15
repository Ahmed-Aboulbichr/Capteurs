using System.Net;

namespace Capteurs.API.Models.DTOs;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessage { get; set; } = new();
    public LoginResponseDto Result { get; set; } = new();
}
