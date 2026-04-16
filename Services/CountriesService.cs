using ServiceContacts;
using ServiceContacts.DTOs;

namespace Services;

public class CountriesService : ICountriesService
{
    public CountryResponseDTO AddCountry(CountryAddRequestDTO country)
    {
        return new CountryResponseDTO();
    }
}
