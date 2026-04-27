using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContacts;
using ServiceContacts.DTOs;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ApplicationDbContext _db;

    public CountriesService(ApplicationDbContext db)
    {
        _db = db;
    }



    public async Task<CountryResponseDTO> AddCountry(CountryAddRequestDTO model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        if (string.IsNullOrWhiteSpace(model.CountryName))
            throw new ArgumentException("Country name cannot be null or empty");

        var exists = await _db.Countries.AnyAsync(c => c.CountryName == model.CountryName);

        if (exists)
            throw new ArgumentException("Country already exists");

        var country = model.ToCountry();
        country.Id = Guid.NewGuid();

        await _db.Countries.AddAsync(country);

        await _db.SaveChangesAsync();

        return country.ToCountryResponse();
    }



    public async Task<List<CountryResponseDTO>> GetAllCountries()
    {
        return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
    }

    public async Task<CountryResponseDTO?> GetCountryById(Guid? id)
    {
        if (id is null) return null;

        Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.Id == id);

        if (country is null) return null;

        return country.ToCountryResponse();
    }
}
