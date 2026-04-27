using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // table name
        modelBuilder.Entity<Country>().ToTable("countries");
        modelBuilder.Entity<Person>().ToTable("peoples");

        // seed data
        var countriesJson = File.ReadAllText("Data/countries.json");
        var countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

        var personsJson = File.ReadAllText("Data/persons.json");
        var persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

        modelBuilder.Entity<Country>().HasData(countries!.ToArray());
        modelBuilder.Entity<Person>().HasData(persons!.ToArray());
    }


}
