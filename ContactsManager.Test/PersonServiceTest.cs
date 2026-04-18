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
        _personService = new PersonService();
        _countriesService = new CountriesService();
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
}
