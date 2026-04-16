using ServiceContacts.DTOs;

namespace ServiceContacts;

public interface ICountriesService
{

    CountryResponseDTO AddCountry(CountryAddRequestDTO country);

}
