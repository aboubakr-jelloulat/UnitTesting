using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContacts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICountriesService, CountriesService>();

builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


app.UseRouting();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}")
    .WithStaticAssets();


app.Run();
