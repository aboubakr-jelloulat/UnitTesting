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
        throw new NotImplementedException();
    }





   
}