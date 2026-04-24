using Microsoft.AspNetCore.Mvc;
using ServiceContacts;
using ServiceContacts.DTOs;
using ServiceContacts.Enums;

namespace ContactsManager.Controllers;

[Route("person")]
// or
//[Route("[controller]")]
public class PersonsController : Controller
{
    private readonly IPersonService     _personService;
    private readonly ICountriesService  _countriesService;

    public PersonsController(IPersonService personService, ICountriesService countriesService)
    {
        _personService = personService;  _countriesService = countriesService;
    }


    [Route("index")]
    //or
    //[Route("[action]")]
    //or
    //[Route("[controller]/[action]")]
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



    [Route("create")]
    [HttpGet]
    public IActionResult Create()
    {
        // Load countries and genders for dropdown options
        ViewBag.Countries = _countriesService.GetAllCountries();
        ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

        return View();
    }

    [Route("create")]
    //or
    //[Route("[action]")]
    [HttpPost]
    public IActionResult Create(PersonAddRequestDTO model)
    {
       
        if (!ModelState.IsValid)
        {
            // Load countries and genders for dropdown options
            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

            return View(model);
        }

        try
        {
            var personResponse = _personService.AddPerson(model);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            // Add general form error (shows at top)
            ModelState.AddModelError(string.Empty, ex.Message);

            // Reload dropdowns
            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

            return View(model);
        }
    }



    [Route("[action]")]
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        var person = _personService.GetPersonById(id);

        if (person is null)
        {
            return NotFound();
        }

        var updateModel = new PersonUpdateRequestDTO
        {
            Id = person.Id,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = Enum.TryParse<GenderOptions>(person.Gender, out var gender)? gender : null,
            CountryId = person.CountryId,
            Address = person.Adress,
            ReceiveNewsLetters = person.ReceiveNewsLetters
        };

        // Load dropdowns
        ViewBag.Countries = _countriesService.GetAllCountries();
        ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

        return View(updateModel);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult Update(PersonUpdateRequestDTO? model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

            return View(model);
        }

        try
        {
            _personService.UpdatePerson(model);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

            return View(model);
        }
    }




}
