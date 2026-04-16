using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts.DTOs;

public class CountryResponseDTO
{
    public Guid Id { get; set; }

    public string? CountryName { get; set; }
}

public static class CountryExtension
{
    public static CountryResponseDTO ToCountryResponse(this Country country) =>
        new CountryResponseDTO() { Id = country.Id, CountryName = country.CountryName };


}

