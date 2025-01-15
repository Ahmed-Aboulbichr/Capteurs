using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Capteurs.API.Data;
using Capteurs.API.Models.Domain;
using Capteurs.API.RepositoriesImpl;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class CapteurRepositoryTests
{
    private readonly Mock<MyDbContext> _mockDbContext;
    private readonly CapteurRepository _capteurRepository;

    public CapteurRepositoryTests()
    {
        _mockDbContext = new Mock<MyDbContext>(new DbContextOptions<MyDbContext>());
        _capteurRepository = new CapteurRepository(_mockDbContext.Object);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCapteur()
    {
        // Arrange
        var capteurId = Guid.NewGuid();
        var capteur = new Capteur { Id = capteurId, Name = "Test Sensor", Description = "This is a test sensor" };

        _mockDbContext.Setup(x => x.Capteurs.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Capteur, bool>>>(), default))
            .ReturnsAsync(capteur);
        _mockDbContext.Setup(x => x.Capteurs.Remove(capteur)).Verifiable();
        _mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _capteurRepository.DeleteAsync(capteurId);

        // Assert
        Assert.Equal(capteurId, result.Id);
        Assert.Equal("Test Sensor", result.Name);
        Assert.Equal("This is a test sensor", result.Description);
        _mockDbContext.Verify(x => x.Capteurs.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Capteur, bool>>>(), default), Times.Once);
        _mockDbContext.Verify(x => x.Capteurs.Remove(capteur), Times.Once);
        _mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCapteurs()
    {
        // Arrange
        var capteurs = new List<Capteur>
        {
            new Capteur { Id = Guid.NewGuid(), Name = "Sensor 1", Description = "Description 1" },
            new Capteur { Id = Guid.NewGuid(), Name = "Sensor 2", Description = "Description 2" },
            new Capteur { Id = Guid.NewGuid(), Name = "Sensor 3", Description = "Description 3" }
        };

        _mockDbContext.Setup(x => x.Capteurs.ToListAsync(default)).ReturnsAsync(capteurs);

        // Act
        var result = await _capteurRepository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Equal(capteurs, result);
        _mockDbContext.Verify(x => x.Capteurs.ToListAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCapteur()
    {
        // Arrange
        var capteurId = Guid.NewGuid();
        var capteur = new Capteur { Id = capteurId, Name = "Test Sensor", Description = "This is a test sensor" };

        _mockDbContext.Setup(x => x.Capteurs.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Capteur, bool>>>(), default))
            .ReturnsAsync(capteur);

        // Act
        var result = await _capteurRepository.GetAsync(capteurId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(capteurId, result.Id);
        Assert.Equal("Test Sensor", result.Name);
        Assert.Equal("This is a test sensor", result.Description);
        _mockDbContext.Verify(x => x.Capteurs.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Capteur, bool>>>(), default), Times.Once);
    }
}
