using Capteurs.API.Data;
using Capteurs.API.Models.Domain;
using Capteurs.API.Repositories;
using Capteurs.API.RepositoriesImpl;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Capteurs.API.Tests.Repositories
{
    public class CapteurRepositoryTests
    {
        private readonly ICapteurRepository capteurRepository;
        private readonly MyDbContext dbContext;

        public CapteurRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            dbContext = new MyDbContext(options);

            dbContext.Capteurs.AddRange(GenerateTestData());
            dbContext.SaveChanges();

            capteurRepository = new CapteurRepository(dbContext);
        }

        [Fact]
        public async Task GetCapteurs()
        {
            // Act
            IEnumerable<Capteur> lstData = await capteurRepository.GetAllAsync();

            // Assert
            Assert.NotNull(lstData);
            Assert.NotEmpty(lstData);
        }

        private List<Capteur> GenerateTestData()
        {
            List<Capteur> lstCapteur = new();
            Random rand = new Random();

            for (int index = 1; index <= 10; index++)
            {
                lstCapteur.Add(new Capteur
                {
                    Id = Guid.NewGuid(),
                    Name = "Capteur-" + index,
                    Description = rand.Next(1, 100) + " Capteur"
                });
            }

            return lstCapteur;
        }
    }
}
