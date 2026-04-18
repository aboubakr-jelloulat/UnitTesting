using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;
using Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        throw new NotImplementedException();
    }
}
