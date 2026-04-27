using ServiceContacts.DTOs;

namespace ServiceContacts;

public interface ICountriesService
{

    Task<CountryResponseDTO> AddCountry(CountryAddRequestDTO model);

    Task<List<CountryResponseDTO>> GetAllCountries();

    Task<CountryResponseDTO?> GetCountryById(Guid? id);

}
