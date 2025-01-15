using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;

namespace Capteurs.API.Repositories;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDto> Login(LoginRequestDto login);
    Task<UserDto> Register(RegistrationRequestDto register);
}
