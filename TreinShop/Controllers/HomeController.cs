using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Xml.Linq;
using TreinShop.Models;

namespace TreinShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ////IStringLocalizer is the key for implementing different languages in your app 
        //Based on injecting the IStringLocalizer<T> abstraction, the string localizer will map
        //the resource file through the value passed to the shared resource (T). This string localizer will then return the mapped value
        // from the resource file based on the name of the string resource you have passed from the respective culture resource file (e.g., HomeController.en-US.resx)..

        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult SetAppLanguage(string lang, string returnUrl) 
        {
            // er wordt een cookie aangemaakt met de naam .AspNetCore.Culture (zie browser cookie)
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}