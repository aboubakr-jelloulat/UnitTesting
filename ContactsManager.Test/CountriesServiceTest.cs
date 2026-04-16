using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using ServiceContacts.DTOs;
using ServiceContacts;
using Services;


namespace ContactsManager.Test;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest(ICountriesService countriesService)
    {
        _countriesService = new CountriesService();
    }

    // When CountryAddRequestDTO is null it should throw ArgumentException

    [Fact]

    public void CountryAddRequestDTO_Null_Test()
    {
        // Arrage
        CountryAddRequestDTO? request = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //  Act
            _countriesService.AddCountry(request);

        });

    }


    // When CountryName is null it should throw ArgumentException

    [Fact]

    public void CountryName_Null_Test()
    {
        // Arrage
        CountryAddRequestDTO? request1 = new() { CountryName = "China" };

        CountryAddRequestDTO? request2 = new() { CountryName = "China" };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //  Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);

        });
    }


    // When CountryName is duplicate it should throw ArgumentException

    [Fact]

    public void CountryNameIsDuplicate_Test()
    {
        // Arrage
        CountryAddRequestDTO? request = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //  Act
            _countriesService.AddCountry(request);

        });

    }


    // When you supply proper country name it should insert add to the existing list of countries 

    [Fact]

    public void CountryProperty_Test()
    {
        // Arrage
        CountryAddRequestDTO? request = new() { CountryName = "Norway"};

        // Act
        CountryResponseDTO response = _countriesService.AddCountry(request);

        // Assert
        Assert.True(response.Id != Guid.Empty);
    }


}
