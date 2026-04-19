using Entities;
using ServiceContacts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts.DTOs;

public class PersonResponseDTO
{
    public Guid Id { get; set; }

    public string PersonName { get; set; } = String.Empty;

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }


    public string? Gender { get; set; }

    public string? Adress { get; set; }

    public Guid? CountryId { get; set; }

    public string? Country { get; set; }

    public bool ReceiveNewsLetters { get; set; }

    public double? Age { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != typeof(PersonResponseDTO))
            return false;

        var personToCompare = (PersonResponseDTO)obj;

        return
            Id == personToCompare.Id &&
            PersonName == personToCompare.PersonName &&
            Email == personToCompare.Email &&
            DateOfBirth == personToCompare.DateOfBirth &&
            Gender == personToCompare.Gender &&
            Adress == personToCompare.Adress &&
            CountryId == personToCompare.CountryId &&
            Country == personToCompare.Country &&
            ReceiveNewsLetters == personToCompare.ReceiveNewsLetters &&
            Age == personToCompare.Age;
    }

    public override int GetHashCode() // avoid Warning
    {
        return base.GetHashCode();
    }

    public PersonUpdateRequestDTO ToPersonUpdateRequest()
    {
        return new PersonUpdateRequestDTO()
        {
            Id = Id,
            PersonName = PersonName,
            Address = Adress,
            CountryId = CountryId,
            DateOfBirth = DateOfBirth,
            Email = Email,
            ReceiveNewsLetters = ReceiveNewsLetters,

            Gender = !string.IsNullOrWhiteSpace(Gender) && Enum.TryParse<GenderOptions>(Gender, true, out var result) ? result : null

        };
    }

}


public static class PersonExtention
{
    public static PersonResponseDTO ToPersonResponse(this Person person)
    {
        return new PersonResponseDTO()
        {
            Id = person.Id,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            Adress = person.Adress,
            CountryId = person.CountryId,
            ReceiveNewsLetters = person.ReceiveNewsLetters,

            //Age
            Age = person.DateOfBirth.HasValue ? CalculateAge(person.DateOfBirth.Value) : null
        };
    }

 
    private static double CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;

        int age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }
}
