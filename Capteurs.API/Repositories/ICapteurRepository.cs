using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;

namespace Capteurs.API.Repositories;

public interface ICapteurRepository
{
    Task<IEnumerable<Capteur>> GetAllAsync();
    Task<Capteur?> GetAsync(Guid id);
    Task<Capteur> AddAsync(Capteur capteur);
    Task<Capteur> DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
