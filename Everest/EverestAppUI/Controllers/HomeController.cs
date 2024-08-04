using DomainServices.Interface;
using EverestAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EverestAppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IProgService _progService;

        public HomeController(ILogger<HomeController> logger,
                              IProgService progService,
                              ICourseService courseService,
                              IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _courseService = courseService;
            _progService = progService;
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

        [HttpGet]
        public async Task<IActionResult> Held()
        {
            var courseList = 
            return View();
        }
    }
}