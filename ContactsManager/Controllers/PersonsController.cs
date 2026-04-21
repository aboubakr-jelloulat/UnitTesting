using ContactsManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ServiceContacts;
using ServiceContacts.DTOs;

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
    public IActionResult Index()
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            {nameof(PersonResponseDTO.PersonName),  "Person Name"},
            {nameof(PersonResponseDTO.Email),       "Email"},
            {nameof(PersonResponseDTO.CountryId),   "Country"},
            {nameof(PersonResponseDTO.Adress), "Address"},
        };


        var persons = _personService.GetAllPersons();

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
