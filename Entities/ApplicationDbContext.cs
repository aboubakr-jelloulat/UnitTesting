using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities;

public class ApplicationDbContext : DbContext
{
    DbSet<Country> countries { get; set; }
    DbSet<Person> peoples { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // table Name
        modelBuilder.Entity<Country>().ToTable("countries");

        modelBuilder.Entity<Person>().ToTable("peoples");

    }

}
