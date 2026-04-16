using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactsManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
