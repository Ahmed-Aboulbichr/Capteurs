using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capteurs.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersionNeutral]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;
    protected ApiResponse _response;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
        _response = new();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await userRepository.Login(model);
        if (loginResponse.User == null || string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username or password is incorrect");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = loginResponse;
        return Ok(_response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
    {
        bool isUserNameUnique =  userRepository.IsUniqueUser(model.UserName);
        if (!isUserNameUnique)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username already exists");
            return BadRequest(_response);
        }
        var user = await userRepository.Register(model);
        if (user is null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Error While Registring");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}

