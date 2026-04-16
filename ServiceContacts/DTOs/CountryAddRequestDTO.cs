using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts.DTOs;

public class CountryAddRequestDTO
{
    public string? CountryName { get; set; }

    public Country ToCountry() => new Country() { CountryName = CountryName };
}
