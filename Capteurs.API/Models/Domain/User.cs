using Microsoft.AspNetCore.Identity;

namespace Capteurs.API.Models.Domain;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
