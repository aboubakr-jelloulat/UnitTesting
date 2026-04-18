using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new();
    }

    

    public CountryResponseDTO AddCountry(CountryAddRequestDTO model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        if (string.IsNullOrWhiteSpace(model.CountryName))
            throw new ArgumentException("Country name cannot be null or empty");

        if (_countries.Any(c => c.CountryName == model.CountryName))
            throw new ArgumentException("Country already exists");

        Country country = model.ToCountry();

        country.Id = Guid.NewGuid();

        _countries.Add(country);

        return country.ToCountryResponse();
    }

    


   
    public List<CountryResponseDTO> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponseDTO? GetCountryById(Guid? id)
    {
        if (id is null) return null;

        Country? country = _countries.FirstOrDefault(c => c.Id == id);

        if (country is null) return null;

        return country.ToCountryResponse();
    }
}
