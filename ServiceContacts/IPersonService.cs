using ServiceContacts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContacts;

public interface IPersonService
{
    PersonResponseDTO AddPerson(PersonAddRequestDTO? model);

    List<PersonResponseDTO> GetAllPersons();

    PersonResponseDTO? GetPersonById(Guid? id);

    List<PersonResponseDTO> GetFiltredPersons(string searchBy, string? searchString);

}
