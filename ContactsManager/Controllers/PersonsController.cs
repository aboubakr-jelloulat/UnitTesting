using ContactsManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ServiceContacts;
using ServiceContacts.DTOs;
using ServiceContacts.Enums;

namespace ContactsManager.Controllers;

public class PersonsController : Controller
{
    private readonly IPersonService     _personService;
    private readonly ICountriesService  _countriesService;

    public PersonsController(IPersonService personService, ICountriesService countriesService)
    {
        _personService = personService;  _countriesService = countriesService;
    }


    [Route("person/index")]
    [Route("/")]
    [HttpGet]
    public IActionResult Index(string searchBy, string? searchString, string sortedBy = nameof(PersonResponseDTO.PersonName),
        SortedOrderOptions sortedOrder = SortedOrderOptions.ASC)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            {nameof(PersonResponseDTO.PersonName),  "Person Name"},
            {nameof(PersonResponseDTO.Email),       "Email"},
            {nameof(PersonResponseDTO.Adress), "Address"},
        };

        ViewBag.CurrentSearchBy     = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewBag.CurrentSortedBy = sortedBy;
        ViewBag.CurrentSortedOrder = sortedOrder;

        var persons = _personService.GetFiltredPersons(searchBy, searchString);

        persons = _personService.GetSortedPersons(persons, sortedBy, sortedOrder);


        var result = persons.Select(p => new PersonViewModel
        {
            Id = p.Id,
            PersonName = p.PersonName,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Address = p.Adress,
            ReceiveNewsLetters = p.ReceiveNewsLetters,

            Age = p.DateOfBirth.HasValue ? DateTime.Now.Year - p.DateOfBirth.Value.Year : null,

            Country = _countriesService.GetCountryById(p.CountryId)?.CountryName  
        }).ToList();

        return View(result);
    }
}
