using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;
using Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit.Abstractions;

namespace Services;

public class PersonService : IPersonService
{
    private readonly List<Person> _people;
    private readonly ICountriesService _countriesService;


    public PersonService()
    {
        _people = new();
        _countriesService = new CountriesService();
    }

    


    private PersonResponseDTO _ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();

        personResponse.Country = _countriesService.GetCountryById(person.CountryId)?.CountryName;

        return personResponse;
    }

    public PersonResponseDTO AddPerson(PersonAddRequestDTO? model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        //if (string.IsNullOrEmpty(model.PersonName))
        //    throw new ArgumentException("Person Name cannot be null or Empty");

        // Model Validations

        model.ValidateModel(); // Extention Methode

        Person person = model.ToPerson();

        person.Id = Guid.NewGuid();

        _people.Add(person);

        

        return _ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponseDTO> GetAllPersons()
    {
        return _people.Select(p => p.ToPersonResponse()).ToList();
    }

    public PersonResponseDTO? GetPersonById(Guid? id)
    {
        if (id is null)
            return null;

        Person? person = _people.FirstOrDefault(p => p.Id == id);

        if (person is null)
            return null;

        return person.ToPersonResponse();
    }

    

    public List<PersonResponseDTO> GetFiltredPersons(string searchBy, string? searchString)
    {
        List<PersonResponseDTO> allPersons = GetAllPersons();

        if (string.IsNullOrWhiteSpace(searchString))
            return allPersons;

        searchString = searchString.ToLower();

        List<PersonResponseDTO> matchingPersons = searchBy switch
        {
            nameof(PersonResponseDTO.PersonName) =>
                allPersons.Where(p => p.PersonName is not null &&
                                      p.PersonName.ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.Email) =>
                allPersons.Where(p => p.Email is not null &&
                                      p.Email.ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.Gender) =>
                allPersons.Where(p => p.Gender is not null &&
                                      p.Gender.ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.Country) =>
                allPersons.Where(p => p.Country is not null &&
                                      p.Country.ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.Adress) =>
                allPersons.Where(p => p.Adress is not null &&
                                      p.Adress.ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.DateOfBirth) =>
                allPersons.Where(p => p.DateOfBirth.HasValue &&
                                      p.DateOfBirth.Value.ToString("yyyy-MM-dd")
                                          .ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.Age) =>
                allPersons.Where(p => p.Age.HasValue &&
                                      p.Age.Value.ToString()
                                          .ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.ReceiveNewsLetters) =>
                allPersons.Where(p =>
                    p.ReceiveNewsLetters.ToString()
                        .ToLower().Contains(searchString)).ToList(),

            nameof(PersonResponseDTO.CountryId) =>
                allPersons.Where(p => p.CountryId.HasValue &&
                                      p.CountryId.Value.ToString()
                                          .ToLower().Contains(searchString)).ToList(),

            _ => allPersons
        };

        return matchingPersons;
    }
}
