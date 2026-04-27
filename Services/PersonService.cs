using Entities;
using Microsoft.EntityFrameworkCore;
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
    private readonly ApplicationDbContext _db;
    private readonly ICountriesService _countriesService;


    public PersonService(ApplicationDbContext db, ICountriesService countriesService, bool initialize = true)
    {
        _countriesService = countriesService;

        _db = db;
    }




    private async Task<PersonResponseDTO> _ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();

        var country = await _countriesService.GetCountryById(person.CountryId);

        personResponse.Country = country?.CountryName;

        return personResponse;
    }


    public async Task<PersonResponseDTO> AddPerson(PersonAddRequestDTO? model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        //if (string.IsNullOrEmpty(model.PersonName))
        //    throw new ArgumentException("Person Name cannot be null or Empty");

        // Model Validations

        model.ValidateModel(); // Extention Methode

        Person person = model.ToPerson();

        person.Id = Guid.NewGuid();

        await _db.AddAsync(person);
        await _db.SaveChangesAsync();
        
        return await _ConvertPersonToPersonResponse(person);
    }

    public async Task<List<PersonResponseDTO>> GetAllPersons()
    {
        var persons = await _db.Persons.ToListAsync();

        var result = await Task.WhenAll(persons.Select(p => _ConvertPersonToPersonResponse(p)));

        return result.ToList();
    }

    public async Task<PersonResponseDTO?> GetPersonById(Guid? id)
    {
        if (id is null)
            return null;

        Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id);

        if (person is null)
            return null;

        return await _ConvertPersonToPersonResponse(person);
    }

    

    public async Task<List<PersonResponseDTO>> GetFiltredPersons(string searchBy, string? searchString)
    {
        List<PersonResponseDTO> allPersons = await GetAllPersons();

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

    public List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> allPersons, string sortedBy, SortedOrderOptions sortOrder)
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

    public async Task<PersonResponseDTO> UpdatePerson(PersonUpdateRequestDTO? model)
    {
        if (model is null)
            throw new ArgumentNullException();

        model.ValidateModel();

        Person? personFromdb = await _db.Persons
            .FirstOrDefaultAsync(p => p.Id == model.Id);

        if (personFromdb is null)
            throw new KeyNotFoundException("Given Person Id is Not matched");

        personFromdb.PersonName = model.PersonName;
        personFromdb.Email = model.Email;
        personFromdb.Adress = model.Address;
        personFromdb.CountryId = model.CountryId;
        personFromdb.DateOfBirth = model.DateOfBirth;
        personFromdb.Gender = model.Gender.ToString();
        personFromdb.ReceiveNewsLetters = model.ReceiveNewsLetters;


        await _db.SaveChangesAsync();

        return await _ConvertPersonToPersonResponse(personFromdb);
    }



    public async Task<bool> DeletePerson(Guid? Id)
    {
        if (Id is null)
            return false;

        Person? personFromdb = await  _db.Persons.FirstOrDefaultAsync(p => p.Id == Id);
        if (personFromdb is null)
            return false;

        _db.Persons.Remove(personFromdb);
        await _db.SaveChangesAsync();

        return true;
    }
}
