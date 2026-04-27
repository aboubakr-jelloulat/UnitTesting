using ServiceContacts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceContacts.Enums;

namespace ServiceContacts;

public interface IPersonService
{
    Task<PersonResponseDTO> AddPerson(PersonAddRequestDTO? model);

    Task<List<PersonResponseDTO>> GetAllPersons();

    Task<PersonResponseDTO?> GetPersonById(Guid? id);

    Task<List<PersonResponseDTO>> GetFiltredPersons(string searchBy, string? searchString);

    List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> allpersons, string sortedBy, SortedOrderOptions SortOrder);


    Task<PersonResponseDTO> UpdatePerson(PersonUpdateRequestDTO? model);

    Task<bool> DeletePerson(Guid? Id);
}
