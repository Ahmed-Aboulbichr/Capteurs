using AutoMapper;
using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;
using Capteurs.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capteurs.API.Controllers.V2;

[ApiController]
[Route("/api/v{version:apiVersion}/capteurs")]
[ApiVersion("2.0")]
public class CapteursController : ControllerBase
{
    private readonly ICapteursService capteursService;
    private readonly IMapper mapper;

    public CapteursController(ICapteursService capteursService, IMapper mapper)
    {
        this.capteursService = capteursService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CapteurDto>>> GetAllCapteursAsync()
    {
        var capteurs = await capteursService.GetAllAsync();
        var capteursDto = mapper.Map<IEnumerable<CapteurDto>>(capteurs);
        return Ok(new { capteursDto, info = "Some update in our api" });
    }

}
