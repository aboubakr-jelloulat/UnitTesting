using System;
using System.Collections.Generic;
using System.Text;

namespace Entities;

public class Person
{
    public int Id { get; set; }

    public string PersonName { get; set; } = String.Empty;
     
    public string? Email { get; set; }
     
    public DateTime? DateOfBirth { get; set; }
     
    public string? Gender { get; set; }
     
    public string? Adress { get; set; }
     
    public Guid? CountryId { get; set; }

    public bool ReceiveNewsLetters { get; set; }

 

}
