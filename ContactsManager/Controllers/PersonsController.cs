using Microsoft.AspNetCore.Mvc;
using ServiceContacts;
using ServiceContacts.DTOs;
using ServiceContacts.Enums;

namespace ContactsManager.Controllers;

[Route("person")]
public class PersonsController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;

    public PersonsController(IPersonService personService, ICountriesService countriesService)
    {
        _personService = personService;
        _countriesService = countriesService;
    }


    [Route("index")]
    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Index(
        string searchBy,
        string? searchString,
        string sortedBy = nameof(PersonResponseDTO.PersonName),
        SortedOrderOptions sortedOrder = SortedOrderOptions.ASC)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(PersonResponseDTO.PersonName), "Person Name" },
            { nameof(PersonResponseDTO.Email),      "Email"       },
            { nameof(PersonResponseDTO.Country),    "Country"     },
            { nameof(PersonResponseDTO.Adress),     "Address"     },
        };

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;
        ViewBag.CurrentSortedBy = sortedBy;
        ViewBag.CurrentSortedOrder = sortedOrder;

        
        var persons = await _personService.GetFiltredPersons(searchBy, searchString);
        persons = _personService.GetSortedPersons(persons, sortedBy, sortedOrder);

        return View(persons);
    }


    [Route("create")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        
        ViewBag.Countries = await _countriesService.GetAllCountries();
        ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

        return View();
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> Create(PersonAddRequestDTO model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = await _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();
            return View(model);
        }

        try
        {
            
            await _personService.AddPerson(model);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Countries = await _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();
            return View(model);
        }
    }


    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var person = await _personService.GetPersonById(id);

        if (person is null)
            return NotFound();

        var updateModel = new PersonUpdateRequestDTO
        {
            Id = person.Id,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = Enum.TryParse<GenderOptions>(person.Gender, out var gender) ? gender : null,
            CountryId = person.CountryId,
            Address = person.Adress,
            ReceiveNewsLetters = person.ReceiveNewsLetters
        };

        ViewBag.Countries = await _countriesService.GetAllCountries();
        ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();

        return View(updateModel);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Update(PersonUpdateRequestDTO? model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = await _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();
            return View(model);
        }

        try
        {
            
            await _personService.UpdatePerson(model);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Countries = await _countriesService.GetAllCountries();
            ViewBag.Genders = Enum.GetValues(typeof(GenderOptions)).Cast<GenderOptions>().ToList();
            return View(model);
        }
    }


    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var person = await _personService.GetPersonById(id);

        if (person is null)
            return RedirectToAction("Index");

        return View(person);
    }

    [Route("[action]/{id}")]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _personService.DeletePerson(id);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
}
