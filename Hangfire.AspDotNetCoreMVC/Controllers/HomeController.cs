using Hangfire.AspDotNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Hangfire.AspDotNetCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Create a list of Person objects
            List<Person> people = new List<Person>();

            // Add some Person objects to the list
            people.Add(new Person { Name = "Alice", Age = 30 });
            people.Add(new Person { Name = "Bob", Age = 25 });
            people.Add(new Person { Name = "Charlie", Age = 35 });

            _logger.LogInformation("Person Count is " + people.Count.ToString());
            _logger.LogInformation(JsonConvert.SerializeObject(people, Formatting.Indented));
            return View();
        }

        public IActionResult Privacy()
        {
            try
            {
                _logger.LogInformation("Privacy Click");
                int[] numbers = { 1, 2, 3, 4, 5 };

                // Trying to access an element outside the bounds of the array
                int sixthNumber = numbers[5];
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Privacy Click Error");
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
