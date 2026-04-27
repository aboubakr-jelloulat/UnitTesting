using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities;

public class Person
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string PersonName { get; set; } = String.Empty;

    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }
     
    public DateTime? DateOfBirth { get; set; }
     
    public string? Gender { get; set; }

    [StringLength(100)]
    public string? Adress { get; set; }
     
    public Guid? CountryId { get; set; }

    public bool ReceiveNewsLetters { get; set; }

 

}
