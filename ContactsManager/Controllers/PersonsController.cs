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
            {nameof(PersonResponseDTO.Country),       "Country"},
            {nameof(PersonResponseDTO.Adress), "Address"},
        };

        ViewBag.CurrentSearchBy     = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewBag.CurrentSortedBy = sortedBy;
        ViewBag.CurrentSortedOrder = sortedOrder;

        var persons = _personService.GetFiltredPersons(searchBy, searchString);

        persons = _personService.GetSortedPersons(persons, sortedBy, sortedOrder);



        return View(persons);
    }
}
