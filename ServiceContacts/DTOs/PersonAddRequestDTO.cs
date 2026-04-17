using Entities;
using ServiceContacts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts.DTOs;

public class PersonAddRequestDTO
{
    public string PersonName { get; set; } = String.Empty;

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public GenderOptions? Gender { get; set; }

    public string? Adress { get; set; }

    public Guid? CountryId { get; set; }

    public bool ReceiveNewsLetters { get; set; }


    public Person ToPerson()
    {
        return new Person()
        {
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            Adress = Adress,
            CountryId = CountryId,
            ReceiveNewsLetters = ReceiveNewsLetters

        };
    }
}
