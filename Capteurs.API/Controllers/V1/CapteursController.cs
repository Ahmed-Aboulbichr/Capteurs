using AutoMapper;
using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;
using Capteurs.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capteurs.API.Controllers.V1;

[ApiController]
[Route("/api/v{version:apiVersion}/capteurs")]
[ApiVersion("1.0")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [ResponseCache(Duration = 30)]
    public async Task<ActionResult<IEnumerable<CapteurDto>>> GetAllCapteursAsync()
    {
        var capteurs = await capteursService.GetAllAsync();
        var capteursDto = mapper.Map<IEnumerable<CapteurDto>>(capteurs);
        return Ok(capteursDto);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(Duration = 30)]
    public async Task<ActionResult<CapteurDto>> GetCapteurById([FromRoute] Guid id)
    {
        var capteurs = await capteursService.GetAsync(id);
        var capteursDto = mapper.Map<CapteurDto>(capteurs);
        return Ok(capteursDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCapteur([FromBody] CapteurDto capteur)
    {
        var resultId = await capteursService.AddAsync(capteur);
        return CreatedAtAction(nameof(GetCapteurById), new { id = resultId }, null);
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CapteurDto>> UpdateRestaurant([FromRoute] Guid id, [FromBody] CapteurDto capteur)
    {
        capteur.Id = id;
        var capteurDto = await capteursService.UpdateAsync(id, capteur);
        return Ok(capteurDto);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCapteur([FromRoute] Guid id)
    {
        await capteursService.DeleteAsync(id);
        return NoContent();
    }




}
