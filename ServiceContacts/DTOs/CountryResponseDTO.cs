using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts.DTOs;

public class CountryResponseDTO
{
    public Guid Id { get; set; }

    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != typeof(CountryResponseDTO))
            return false;

        var countryToCompare = obj as CountryResponseDTO;

        return countryToCompare.Id == Id
            && countryToCompare.CountryName == CountryName;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

public static class CountryExtension
{
    public static CountryResponseDTO ToCountryResponse(this Country country) =>
        new CountryResponseDTO() { Id = country.Id, CountryName = country.CountryName };


}

