using Entities;
using ServiceContacts.Enums;
using System.ComponentModel.DataAnnotations;

public class PersonAddRequestDTO
{
    [Required(ErrorMessage = "Person name can't be blank")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Person name must be between 3 and 50 characters")]
    public string PersonName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    public GenderOptions? Gender { get; set; }

    [StringLength(100, ErrorMessage = "Address can't exceed 100 characters")]
    public string? Address { get; set; }   

    public Guid? CountryId { get; set; }

    public bool ReceiveNewsLetters { get; set; }

   

    public Person ToPerson()
    {
        return new Person()
        {
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender?.ToString(),
            Adress = Address,
            CountryId = CountryId,
            ReceiveNewsLetters = ReceiveNewsLetters

        };
    }
}
