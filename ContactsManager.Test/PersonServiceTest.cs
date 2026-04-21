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

    public PersonServiceTest()
    {
        _personService = new PersonService(false);
        _countriesService = new CountriesService(false);
    }

    public GenderOptions? GenderOption { get; private set; }


    /*
        A good test name answers 3 questions:

            What method? → AddCountry
            What situation? → ValidInput
            What result? → ReturnsCountry
    */


    #region AddPerson

    [Fact]
    public void AddPerson_NullValue_ThrowArgumentNullException()
    {
        // Arrange 

        PersonAddRequestDTO? personAddRequest = null;

        // Act

        Action act = () => _personService.AddPerson(personAddRequest);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }


    [Fact]
    public void AddPerson_PersonNameIsNull_ThrowArgumentException()
    {
        // Arrange 

        PersonAddRequestDTO? personAddRequest = new()
        {
            PersonName = null
        };

        // Act

        Action act = () => _personService.AddPerson(personAddRequest);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void AddPerson_ShouldBeInPersonList_ThrowException()
    {
        // Arrange 

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

        // Act

        PersonResponseDTO personResponse =  _personService.AddPerson(personAddRequest);
        List<PersonResponseDTO> allPersonList = _personService.GetAllPersons();

        // Assert
        Assert.NotEqual(personResponse.Id, Guid.Empty);
        Assert.Contains(personResponse, allPersonList);
    }


    #endregion


    #region GetPersonById

    [Fact]

    public void GetPersonById_IdIsNull_ReturnNull()
    {
        // Arrange
        Guid? Id = null;

        // Act
        PersonResponseDTO? personResponse = _personService.GetPersonById(Id);

        // Assert
        Assert.Null(personResponse);
    }

    [Fact]
    public void GetPersonById_ValidInput_CanBeRetrievedById()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO() { CountryName = "Sweden"};

        CountryResponseDTO countryResponse = _countriesService.AddCountry(countryAddRequest);

        // Arrange
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

        // Act

        PersonResponseDTO addedPerson = _personService.AddPerson(personAddRequest);

        PersonResponseDTO? retrievedPerson = _personService.GetPersonById(addedPerson.Id);

        // Assert
        Assert.NotNull(retrievedPerson);
        Assert.Equal(retrievedPerson, addedPerson);
    }


    #endregion

    #region GetAllPersons

    [Fact]
    public void GetAllPersons_Initial_ShouldBeEmpty()
    {
        // Act
        var people = _personService.GetAllPersons();

        // Assert
        Assert.Empty(people);
    }

    [Fact]

    public void GetAllPersons_AfterAddingPersons_ReturnsListOfPerson()
    {
        // Act

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
            var response = _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = _personService.GetAllPersons();

        foreach (var person in persons)
        {
            Assert.Contains(person, expected);
        }


    }

    #endregion

    #region GetFiltredPersons


    [Fact]
    public void GetFiltredPersons_EmptySearch_ReturnAllPersons()
    {

        // Act

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
            var response = _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = _personService.GetFiltredPersons(nameof(Person.PersonName), String.Empty);

        foreach (var person in persons)
        {
            Assert.Contains(person, expected);
        }

    }



    // Add Person ; and search by person Name ; should return the matching Person
    [Fact]
    public void GetFiltredPersons_SearchByPersonName_ReturnMatchingPerson()
    {

        // Act

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
            var response = _personService.AddPerson(p);
            expected.Add(response);
        }

        var persons = _personService.GetFiltredPersons(nameof(Person.PersonName), "Aboubakr");

        foreach (var person in persons)
        {
            if (person.PersonName is not null && person.PersonName.Contains("abou", StringComparison.OrdinalIgnoreCase))
                Assert.Contains(person, expected);
        }

    }


    #endregion

    #region GetSortedPersons


    [Fact]
    public void GetSortedPersons_SortingByPersonNameASCandDESC_ReturnMatchingPerson()
    {

        // Act

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
            var response = _personService.AddPerson(p);
            expected.Add(response);
        }

        var Sortedpersons = _personService.GetSortedPersons(_personService.GetAllPersons(), nameof(Person.PersonName), SortedOrderOptions.DESC);

        var expectedSortedPerson = _personService.GetAllPersons().OrderByDescending(p => p.PersonName).ToList();


        // Assert
        for (int i = 0; i < expectedSortedPerson.Count; ++i)
            Assert.Equal(expectedSortedPerson[i], Sortedpersons[i]);

    }


    #endregion

    #region UpdatePerson

    [Fact]
    public void UpdatePerson_NullArgument_ThrowException()
    {
        // Arrange
        PersonUpdateRequestDTO? personUpdateRequest = null;

        // Act
        Action act = () => _personService.UpdatePerson(personUpdateRequest);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }



    [Fact]
    public void UpdatePerson_InvalidPersonID_ThrowException()
    {
        // Arrange
        PersonUpdateRequestDTO? personUpdateRequest = new()
        {
            Id = Guid.NewGuid()
        };

        // Act
        Action act = () => _personService.UpdatePerson(personUpdateRequest);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }


    [Fact]
    public void UpdatePerson_PersonNameIsNull_ThrowException()
    {
        // Arrange

        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"
            
        };

        CountryResponseDTO countryResponse = _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = _personService.AddPerson(personAddRequest);


        PersonUpdateRequestDTO? personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = null;

        // Act
        Action act = () => _personService.UpdatePerson(personUpdateRequest);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }


    // Add person; update Name and Email ; Return Updated Values
    [Fact]
    public void UpdatePerson_ValidArgument_ReturnTheUpdated()
    {
        // Arrange

        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"

        };

        CountryResponseDTO countryResponse = _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female,
            Email = "Linta@gmail.com"
        };

        PersonResponseDTO personResponse = _personService.AddPerson(personAddRequest);


        PersonUpdateRequestDTO? personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = "Estina";
        personUpdateRequest.Email = "Estina@gmail.com";

        

        // Act
        PersonResponseDTO updatedPerson = _personService.UpdatePerson(personUpdateRequest);
        PersonResponseDTO? personFromdb = _personService.GetPersonById(updatedPerson.Id);


        //Assert

        //Assert.Equal(personUpdateRequest.PersonName, updatedPerson.PersonName);
        //Assert.Equal(personUpdateRequest.Email, updatedPerson.Email);

        Assert.Equal(updatedPerson, personFromdb);

    }





    #endregion


    #region DeletePerson

    [Fact]
    public void DeletePerson_InvalidPersonId_ThrowException()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"

        };

        CountryResponseDTO countryResponse = _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = _personService.AddPerson(personAddRequest);

         


        //Assert
        Assert.False(_personService.DeletePerson(Guid.NewGuid()));
    }


    [Fact]
    public void DeletePerson_validPersonId_PersonDeleteReturnTrue()
    {
        CountryAddRequestDTO countryAddRequest = new CountryAddRequestDTO()
        {
            CountryName = "Lithuania"

        };

        CountryResponseDTO countryResponse = _countriesService.AddCountry(countryAddRequest);

        PersonAddRequestDTO personAddRequest = new()
        {
            CountryId = countryResponse.Id,
            PersonName = "Linta evatua",
            Address = "xx Streetm Lithuania",
            Gender = GenderOptions.Female
        };

        PersonResponseDTO personResponse = _personService.AddPerson(personAddRequest);




        //Assert
        Assert.True(_personService.DeletePerson(personResponse.Id));
    }

    #endregion

}
