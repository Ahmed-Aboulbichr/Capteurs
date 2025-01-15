﻿using Capteurs.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capteurs.API.Data;

public class MyDbContext : IdentityDbContext<User>
{
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
	{

	}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  
    }
    public DbSet<User> MyUsers { get; set; }

	public DbSet<Capteur> Capteurs { get; set; }
}
