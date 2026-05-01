using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;
using ServiceContacts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsManager.Test;

public class PersonServiceTest
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ApplicationDbContext _db;

    public PersonServiceTest(ApplicationDbContext db)
    {
        _db = db;

        _countriesService = new CountriesService(_db);
        _personService = new PersonService(_db, _countriesService);
    }

    public GenderOptions? GenderOption { get; private set; }

    #region AddPerson

    [Fact]
    public async Task AddPerson_NullValue_ThrowArgumentNullException()
    {
        PersonAddRequestDTO? personAddRequest = null;

        Func<Task> act = async () => await _personService.AddPerson(personAddRequest);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task AddPerson_PersonNameIsNull_ThrowArgumentException()
    {
        PersonAddRequestDTO? personAddRequest = new()
        {
            PersonName = null
        };

        Func<Task> act = async () => await _personService.AddPerson(personAddRequest);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task AddPerson_ShouldBeInPersonList_ThrowException()
    {
        PersonAddRequestDTO? personAddRequest = new()
        {
            PersonName = "Aboubakr",
            Email = "ajelloul@gmail.com",
            Address = "Java Street, Stockholm",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        PersonResponseDTO personResponse = await _personService.AddPerson(personAddRequest);
        List<PersonResponseDTO> allPersonList = await _personService.GetAllPersons();

        Assert.NotEqual(personResponse.Id, Guid.Empty);
        Assert.Contains(personResponse, allPersonList);
    }

    #endregion


    #region GetPersonById

    [Fact]
    public async Task GetPersonById_IdIsNull_ReturnNull()
    {
        Guid? Id = null;

        PersonResponseDTO? personResponse = await _personService.GetPersonById(Id);

        Assert.Null(personResponse);
    }

    [Fact]
    public async Task GetPersonById_ValidInput_CanBeRetrievedById()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO() { CountryName = "Sweden" };

        CountryResponseDTO countryResponse = await _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO? personAddRequest = new()
        {
            PersonName = "Aboubakr",
            Email = "ajelloul@gmail.com",
            Address = "Java Street, Stockholm",
            CountryId = countryResponse.Id,
            DateOfBirth = DateTime.Now,
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        PersonResponseDTO addedPerson = await _personService.AddPerson(personAddRequest);

        PersonResponseDTO? retrievedPerson = await _personService.GetPersonById(addedPerson.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(retrievedPerson, addedPerson);
    }

    #endregion

    #region GetAllPersons

    [Fact]
    public async Task GetAllPersons_Initial_ShouldBeEmpty()
    {
        var people = await _personService.GetAllPersons();

        Assert.Empty(people);
    }

    [Fact]
    public async Task GetAllPersons_AfterAddingPersons_ReturnsListOfPerson()
    {
        List<PersonAddRequestDTO> addPersons = new()
        {
            new()
            {
                PersonName = "Aboubakr",
                Email = "ajelloul@gmail.com",
                Address = "Java Street, Stockholm",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Ayoub",
                Email = "abouatr@gmail.com",
                Address = "Helsinki Street, Helsinki",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Emma",
                Email = "Emma@gmail.com",
                Address = "Rue Esquermoise , Lille",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true
            }
        };

        var expected = new List<PersonResponseDTO>();

        foreach (var p in addPersons)
        {
            var response = await _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = await _personService.GetAllPersons();

        foreach (var person in persons)
        {
            Assert.Contains(person, expected);
        }
    }

    #endregion

    #region GetFiltredPersons

    [Fact]
    public async Task GetFiltredPersons_EmptySearch_ReturnAllPersons()
    {
        List<PersonAddRequestDTO> addPersons = new()
        {
            new()
            {
                PersonName = "Aboubakr",
                Email = "ajelloul@gmail.com",
                Address = "Java Street, Stockholm",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Ayoub",
                Email = "abouatr@gmail.com",
                Address = "Helsinki Street, Helsinki",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Emma",
                Email = "Emma@gmail.com",
                Address = "Rue Esquermoise , Lille",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true
            }
        };

        var expected = new List<PersonResponseDTO>();

        foreach (var p in addPersons)
        {
            var response = await _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = await _personService.GetFiltredPersons(nameof(Person.PersonName), String.Empty);

        foreach (var person in persons)
        {
            Assert.Contains(person, expected);
        }
    }

    [Fact]
    public async Task GetFiltredPersons_SearchByPersonName_ReturnMatchingPerson()
    {
        List<PersonAddRequestDTO> addPersons = new()
        {
            new()
            {
                PersonName = "Aboubakr",
                Email = "ajelloul@gmail.com",
                Address = "Java Street, Stockholm",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Ayoub",
                Email = "abouatr@gmail.com",
                Address = "Helsinki Street, Helsinki",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Emma",
                Email = "Emma@gmail.com",
                Address = "Rue Esquermoise , Lille",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true
            }
        };

        var expected = new List<PersonResponseDTO>();

        foreach (var p in addPersons)
        {
            var response = await _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = await _personService.GetFiltredPersons(nameof(Person.PersonName), "Aboubakr");

        foreach (var person in persons)
        {
            if (person.PersonName is not null && person.PersonName.Contains("abou", StringComparison.OrdinalIgnoreCase))
                Assert.Contains(person, expected);
        }
    }

    #endregion

    #region GetSortedPersons

    [Fact]
    public async Task GetSortedPersons_SortingByPersonNameASCandDESC_ReturnMatchingPerson()
    {
        List<PersonAddRequestDTO> addPersons = new()
        {
            new()
            {
                PersonName = "Aboubakr",
                Email = "ajelloul@gmail.com",
                Address = "Java Street, Stockholm",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Ayoub",
                Email = "abouatr@gmail.com",
                Address = "Helsinki Street, Helsinki",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
            new()
            {
                PersonName = "Emma",
                Email = "Emma@gmail.com",
                Address = "Rue Esquermoise , Lille",
                CountryId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true
            }
        };

        var expected = new List<PersonResponseDTO>();

        foreach (var p in addPersons)
        {
            var response = await _personService.AddPerson(p);
            expected.Add(response);
        }

        var allPersons = await _personService.GetAllPersons();

        var Sortedpersons = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortedOrderOptions.DESC);

        var expectedSortedPerson = allPersons.OrderByDescending(p => p.PersonName).ToList();

        for (int i = 0; i < expectedSortedPerson.Count; ++i)
            Assert.Equal(expectedSortedPerson[i], Sortedpersons[i]);
    }

    #endregion

    #region UpdatePerson

    [Fact]
    public async Task UpdatePerson_NullArgument_ThrowException()
    {
        PersonUpdateRequestDTO? personUpdateRequest = null;

        Func<Task> act = async () => await _personService.UpdatePerson(personUpdateRequest);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task UpdatePerson_InvalidPersonID_ThrowException()
    {
        PersonUpdateRequestDTO? personUpdateRequest = new()
        {
            Id = Guid.NewGuid()
        };

        Func<Task> act = async () => await _personService.UpdatePerson(personUpdateRequest);

        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }

    [Fact]
    public async Task UpdatePerson_PersonNameIsNull_ThrowException()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"
        };

        CountryResponseDTO countryResponse = await _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequestDTO? personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = null;

        Func<Task> act = async () => await _personService.UpdatePerson(personUpdateRequest);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task UpdatePerson_ValidArgument_ReturnTheUpdated()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"
        };

        CountryResponseDTO countryResponse = await _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female,
            Email = "Linta@gmail.com"
        };

        PersonResponseDTO personResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequestDTO? personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = "Estina";
        personUpdateRequest.Email = "Estina@gmail.com";

        PersonResponseDTO updatedPerson = await _personService.UpdatePerson(personUpdateRequest);
        PersonResponseDTO? personFromdb = await _personService.GetPersonById(updatedPerson.Id);

        Assert.Equal(updatedPerson, personFromdb);
    }

    #endregion


    #region DeletePerson

    [Fact]
    public async Task DeletePerson_InvalidPersonId_ThrowException()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"
        };

        CountryResponseDTO countryResponse = await _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = await _personService.AddPerson(personAddRequest);

        Assert.False(await _personService.DeletePerson(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeletePerson_validPersonId_PersonDeleteReturnTrue()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"
        };

        CountryResponseDTO countryResponse = await _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = await _personService.AddPerson(personAddRequest);

        Assert.True(await _personService.DeletePerson(personResponse.Id));
    }

    #endregion
}