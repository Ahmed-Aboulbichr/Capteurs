using AutoMapper;
using Capteurs.API.Data;
using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capteurs.API.RepositoriesImpl;

public class UserRepository : IUserRepository
{
    private readonly MyDbContext myDbContext;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private string secretKey;

    public UserRepository(MyDbContext myDbContext,
        UserManager<User> userManager,
        IMapper mapper,
        IConfiguration configuration)
    {
        this.myDbContext = myDbContext;
        this.userManager = userManager;
        this.mapper = mapper;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret")!;
    }
    public bool IsUniqueUser(string username)
    {
        var user = myDbContext.MyUsers.FirstOrDefault(x => x.UserName == username);
        return user is null;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto login)
    {
        var user = myDbContext.MyUsers.FirstOrDefault(x => x.Email!.ToLower() == login.userName.ToLower());
        bool isValid = await userManager.CheckPasswordAsync(user, login.password);
        if(user is null || isValid == false)
        {
            return new LoginResponseDto()
            {
                Token = "",
                User = null
            };
        }
        // if user was found we generate the Token
        var roles = await userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()), 
            }),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var loginResponse = new LoginResponseDto()
        {
            Token = tokenHandler.WriteToken(token),
            User = mapper.Map<UserDto>(user),
            Role = roles.FirstOrDefault(),
        };
         return loginResponse;
    }



    public async Task<UserDto> Register(RegistrationRequestDto register)
    {
        User user = new()
        {
            UserName = register.UserName,
            Email = register.UserName,
            NormalizedEmail = register.UserName.ToUpper(),
            Name = register.Name
        };
        try
        {
            var result = await userManager.CreateAsync(user, register.Password);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
                var userToReturn = myDbContext.Users.FirstOrDefault(u => u.UserName == register.UserName);
                return mapper.Map<UserDto>(userToReturn);
            }
        }catch(Exception ex)
        {

        }
        return new();
    }
}
