using ServiceContacts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICountriesService, CountriesService>();

builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();


app.UseRouting();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}")
    .WithStaticAssets();


app.Run();
