using AutoMapper;
using Capteurs.API.Exceptions;
using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;

namespace Capteurs.API.Services
{
    public class CapteursService : ICapteursService
    {
        private readonly ICapteurRepository capteurRepository;
        private readonly ILogger<CapteursService> logger;
        private readonly IMapper mapper;

        public CapteursService(ICapteurRepository capteurRepository,
            ILogger<CapteursService> logger,
            IMapper mapper)
        {
            this.capteurRepository = capteurRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<Guid> AddAsync(CapteurDto capteurDto)
        {
            logger.LogInformation("Creating new Capteur {@Capteur}", capteurDto);
            var capteur = mapper.Map<Capteur>(capteurDto);
            await capteurRepository.AddAsync(capteur);
            return capteur.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogInformation("Delete  Capteur {@Capteur}", id);
            await capteurRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CapteurDto>> GetAllAsync()
        {
            logger.LogInformation("Get All Sensors");
            var capteur = await capteurRepository.GetAllAsync();
            var capteurDto = mapper.Map<IEnumerable<CapteurDto>>(capteur);
            return capteurDto;
        }

        public async Task<CapteurDto> GetAsync(Guid id)
        {
            logger.LogInformation("Get Capteur {@Capteur}", id);
            var capteur = await capteurRepository.GetAsync(id);
            if (capteur is null) throw new NotFoundException(nameof(Capteur), id.ToString());
            var capteurDto = mapper.Map<CapteurDto>(capteur);
            return capteurDto;
        }

        public async Task<CapteurDto> UpdateAsync(Guid id, CapteurDto capteurDto)
        {
            logger.LogInformation("Updating  capteur {id} : {@UpdatedRestaurant}", id, capteurDto);
            var capteur = await capteurRepository.GetAsync(id) ??
                throw new NotFoundException($"Capteur", id.ToString());

            mapper.Map(capteurDto, capteur);
            await capteurRepository.SaveChangesAsync();
            return capteurDto;
        }
    }
}
