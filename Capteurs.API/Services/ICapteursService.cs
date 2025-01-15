using Capteurs.API.Models.DTOs;

namespace Capteurs.API.Services;

public interface ICapteursService
{
    Task<IEnumerable<CapteurDto>> GetAllAsync();
    Task<CapteurDto> GetAsync(Guid id);
    Task<Guid> AddAsync(CapteurDto capteur);
    Task DeleteAsync(Guid id);
    Task<CapteurDto> UpdateAsync(Guid id, CapteurDto capteur);
}
