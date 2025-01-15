using AutoMapper;
using Capteurs.API.Data;
using Capteurs.API.Exceptions;
using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;
using Capteurs.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Capteurs.API.RepositoriesImpl
{
    public class CapteurRepository : ICapteurRepository
    {
        private readonly MyDbContext myDbContext;

        public CapteurRepository(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }
        public async Task<Capteur> AddAsync(Capteur capteur)
        {
            capteur.Id = Guid.NewGuid();
            await myDbContext.AddAsync(capteur);
            await myDbContext.SaveChangesAsync();
            return capteur;
        }

        public async Task<Capteur> DeleteAsync(Guid id)
        {
            var capteur = await myDbContext.Capteurs.FirstOrDefaultAsync(x => x.Id == id);

            if (capteur is null)
            {
                throw new NotFoundException(nameof(Capteur), id.ToString());
            }
            myDbContext.Capteurs.Remove(capteur);
            await myDbContext.SaveChangesAsync();
            return capteur;
        }

        public async Task<IEnumerable<Capteur>> GetAllAsync()
        {
            var capteurs = await myDbContext.Capteurs.ToListAsync();
            return capteurs;
        }

        public async Task<Capteur?> GetAsync(Guid id)
        {
            var capteur = await myDbContext.Capteurs.FirstOrDefaultAsync(obj => obj.Id == id);
            return capteur;
        }

        public async Task SaveChangesAsync()
        {
            await myDbContext.SaveChangesAsync();
        }
    }
}
