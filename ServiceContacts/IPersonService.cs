using ServiceContacts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceContacts.Enums;

namespace ServiceContacts;

public interface IPersonService
{
    PersonResponseDTO AddPerson(PersonAddRequestDTO? model);

    List<PersonResponseDTO> GetAllPersons();

    PersonResponseDTO? GetPersonById(Guid? id);

    List<PersonResponseDTO> GetFiltredPersons(string searchBy, string? searchString);


    List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> allpersons, string sortedBy, SortedOrderOptions SortOrder);


    PersonResponseDTO UpdatePerson(PersonUpdateRequestDTO? model);
}
