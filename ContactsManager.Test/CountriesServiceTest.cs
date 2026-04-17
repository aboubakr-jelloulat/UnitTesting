using System;
using Xunit;
using ServiceContacts.DTOs;
using ServiceContacts;
using Services;

namespace ContactsManager.Test;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
    }

    #region AddCountryUniteTest

    // Null request -> should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequestDTO? request = null;

        // Act
        Action act = () => _countriesService.AddCountry(request);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    // Null or empty country name -> should throw ArgumentException
    [Fact]
    public void AddCountry_NullCountryName_ThrowsArgumentException()
    {
        // Arrange
        var request = new CountryAddRequestDTO { CountryName = null };

        // Act
        Action act = () => _countriesService.AddCountry(request);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Duplicate country -> should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName_ThrowsArgumentException()
    {
        // Arrange
        var request = new CountryAddRequestDTO { CountryName = "China" };

        _countriesService.AddCountry(request); // first insert

        // Act
        Action act = () => _countriesService.AddCountry(request);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Valid request -> should return response with non empty Id
    [Fact]
    public void AddCountry_ValidRequest_ReturnsCountryResponse()
    {
        // Arrange
        var request = new CountryAddRequestDTO { CountryName = "Norway" };

        // Act
        var response = _countriesService.AddCountry(request);

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    #endregion


    #region GetAllCountriesUniteTest

    // Empty list initially
    [Fact]
    public void GetAllCountries_Initial_ShouldBeEmpty()
    {
        // Act
        var countries = _countriesService.GetAllCountries();

        // Assert
        Assert.Empty(countries);
    }

    // After adding → should return all countries
    [Fact]
    public void GetAllCountries_AfterAddingCountries_ReturnsList()
    {
        // Arrange
        var addCountries = new List<CountryAddRequestDTO>
        {
            new() { CountryName = "Sweden" },
            new() { CountryName = "Estonia" },
            new() { CountryName = "Finland" }
        };

        var expected = new List<CountryResponseDTO>();

        // Act
        foreach (var c in addCountries)
        {
            var response = _countriesService.AddCountry(c);
            expected.Add(response);
        }

        var countries = _countriesService.GetAllCountries();

        // Assert
        Assert.Equal(expected.Count, countries.Count);

        foreach (var item in expected)
        {
            //Assert.Contains(countries, c => c.Id == item.Id);

            // or override the Equal, because Equal is compare the refrences 

            Assert.Contains(item, countries); // Contains by default use Equal
        }
    }

    #endregion
}
