using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;
using ServiceContacts.Enums;
using Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;

namespace Services;

public class PersonService : IPersonService
{
    private readonly List<Person> _people;
    private readonly ICountriesService _countriesService;


    public PersonService(ICountriesService countriesService, bool initialize = true)
    {
        _countriesService = countriesService;

        _people = new();

        if (initialize)
        {
            _people.AddRange(new List<Person>()
            {
                new ()
                {
                    Id = Guid.NewGuid(),
                    PersonName = "John Smith",
                    Adress = "Helsinki, Finland",
                    CountryId = Guid.Parse("7F516B57-8115-4BC6-8720-EC7EAE4F93C8"),
                    DateOfBirth = new DateTime(1990, 8, 22),
                    Email = "john.smith@example.com",
                    Gender = GenderOptions.Male.ToString(),
                    ReceiveNewsLetters = false
                },

                new ()
                {
                    Id = Guid.NewGuid(),
                    PersonName = "Li Wei",
                    Adress = "Shanghai, China",
                    CountryId = Guid.Parse("CE4FB821-68F4-4C06-B16F-7D44647F2D41"),
                    DateOfBirth = new DateTime(1988, 3, 10),
                    Email = "li.wei@example.com",
                    Gender = GenderOptions.Male.ToString(),
                    ReceiveNewsLetters = true
                },

                new ()
                {
                    Id = Guid.NewGuid(),
                    PersonName = "Anna Kask",
                    Adress = "Tallinn, Estonia",
                    CountryId = Guid.Parse("40EA2121-B73C-4DCE-8D95-368846A15ABE"),
                    DateOfBirth = new DateTime(1997, 11, 5),
                    Email = "anna.kask@example.com",
                    Gender = GenderOptions.Female.ToString(),
                    ReceiveNewsLetters = false
                }

            });
        }

        



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
        return _people.Select(p => _ConvertPersonToPersonResponse(p)).ToList();
    }

    public PersonResponseDTO? GetPersonById(Guid? id)
    {
        if (id is null)
            return null;

        Person? person = _people.FirstOrDefault(p => p.Id == id);

        if (person is null)
            return null;

        return _ConvertPersonToPersonResponse(person);
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

    public List<PersonResponseDTO> GetSortedPersons(
    List<PersonResponseDTO> allPersons,
    string sortedBy,
    SortedOrderOptions sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortedBy))
            return allPersons;

        List<PersonResponseDTO> sortedPersons = (sortedBy, sortOrder) switch
        {
            
            (nameof(PersonResponseDTO.PersonName), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponseDTO.PersonName), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            
            (nameof(PersonResponseDTO.Email), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.Email).ToList(),

            (nameof(PersonResponseDTO.Email), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.Email).ToList(),

            
            (nameof(PersonResponseDTO.DateOfBirth), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.DateOfBirth).ToList(),

            (nameof(PersonResponseDTO.DateOfBirth), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

            
            (nameof(PersonResponseDTO.Age), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.Age).ToList(),

            (nameof(PersonResponseDTO.Age), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.Age).ToList(),

            
            (nameof(PersonResponseDTO.Country), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.Country).ToList(),

            (nameof(PersonResponseDTO.Country), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.Country).ToList(),

            
            (nameof(PersonResponseDTO.Gender), SortedOrderOptions.ASC) =>
                allPersons.OrderBy(p => p.Gender).ToList(),

            (nameof(PersonResponseDTO.Gender), SortedOrderOptions.DESC) =>
                allPersons.OrderByDescending(p => p.Gender).ToList(),

            
            _ => allPersons
        };

        return sortedPersons;
    }

    public PersonResponseDTO UpdatePerson(PersonUpdateRequestDTO? model)
    {
        if (model is null)
            throw new ArgumentNullException();

        // Extention Methode To Validate All Properties
        model.ValidateModel();

        Person? personFromdb = _people.FirstOrDefault(p => p.Id == model.Id );
        if (personFromdb is null)
            throw new ArgumentNullException("Given Person Id is Not matched");


        personFromdb.Id = model.Id;
        personFromdb.PersonName = model.PersonName;
        personFromdb.Email = model.Email;
        personFromdb.Adress = model.Address;
        personFromdb.CountryId = model.CountryId;
        personFromdb.DateOfBirth = model.DateOfBirth;
        personFromdb.Gender = model.Gender.ToString();
        personFromdb.ReceiveNewsLetters = model.ReceiveNewsLetters;
        

        return _ConvertPersonToPersonResponse(personFromdb);
    }



    public bool DeletePerson(Guid? Id)
    {
        if (Id is null)
            return false;

        Person? personFromdb = _people.FirstOrDefault(p => p.Id == Id);
        if (personFromdb is null)
            return false;

        return _people.Remove(personFromdb);
    }
}
