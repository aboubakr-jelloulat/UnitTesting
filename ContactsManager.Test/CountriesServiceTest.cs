using Entities;
using ServiceContacts;
using ServiceContacts.DTOs;
using Services;
using System;
using Xunit;

namespace ContactsManager.Test;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly ApplicationDbContext _db;

    public CountriesServiceTest(ApplicationDbContext db)
    {
        _db = db;
        _countriesService = new CountriesService(_db);
    }

    #region AddCountryUniteTest

    [Fact]
    public async Task AddCountry_NullRequest_ThrowsArgumentNullException()
    {
        CountryAddRequestDTO? request = null;

        Func<Task> act = async () => await _countriesService.AddCountry(request!);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task AddCountry_NullCountryName_ThrowsArgumentException()
    {
        var request = new CountryAddRequestDTO { CountryName = null };

        Func<Task> act = async () => await _countriesService.AddCountry(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task AddCountry_DuplicateCountryName_ThrowsArgumentException()
    {
        var request = new CountryAddRequestDTO { CountryName = "China" };

        await _countriesService.AddCountry(request);

        Func<Task> act = async () => await _countriesService.AddCountry(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task AddCountry_ValidRequest_ReturnsCountryResponse()
    {
        var request = new CountryAddRequestDTO { CountryName = "Norway" };

        var response = await _countriesService.AddCountry(request);

        Assert.NotEqual(Guid.Empty, response.Id);
    }

    #endregion


    #region GetAllCountriesUniteTest

    [Fact]
    public async Task GetAllCountries_Initial_ShouldBeEmpty()
    {
        var countries = await _countriesService.GetAllCountries();

        Assert.Empty(countries);
    }

    [Fact]
    public async Task GetAllCountries_AfterAddingCountries_ReturnsList()
    {
        var addCountries = new List<CountryAddRequestDTO>
        {
            new() { CountryName = "Sweden" },
            new() { CountryName = "Estonia" },
            new() { CountryName = "Finland" }
        };

        var expected = new List<CountryResponseDTO>();

        foreach (var c in addCountries)
        {
            var response = await _countriesService.AddCountry(c);
            expected.Add(response);
        }

        var countries = await _countriesService.GetAllCountries();

        Assert.Equal(expected.Count, countries.Count);

        foreach (var item in expected)
        {
            Assert.Contains(item, countries);
        }
    }

    #endregion


    #region GetCountryById

    [Fact]
    public async Task GetCountryById_NullId_ReturnsNull()
    {
        Guid? id = null;

        var response = await _countriesService.GetCountryById(id);

        Assert.Null(response);
    }

    [Fact]
    public async Task AddCountry_ValidInput_CanBeRetrievedById()
    {
        var request = new CountryAddRequestDTO
        {
            CountryName = "Palestine"
        };

        var addedCountry = await _countriesService.AddCountry(request);
        var retrievedCountry = await _countriesService.GetCountryById(addedCountry.Id);

        Assert.NotNull(retrievedCountry);
        Assert.Equal(addedCountry, retrievedCountry);
    }

    #endregion
}
