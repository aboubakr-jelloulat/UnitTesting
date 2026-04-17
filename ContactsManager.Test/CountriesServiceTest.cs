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
}