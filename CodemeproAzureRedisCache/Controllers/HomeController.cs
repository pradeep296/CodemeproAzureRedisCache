using CodemeproAzureRedisCache.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CodemeproAzureRedisCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var cachedData = await _cache.GetStringAsync("CodeMeproRedisData");
            if (cachedData == null)
            {
                // Data not found in cache, fetch from the database
                var data = await GetDummyEmployeesAsync();

                // Store data in cache for future use
                await _cache.SetStringAsync("CodeMeproRedisData", JsonConvert.SerializeObject(data));
                cachedData = JsonConvert.SerializeObject(data);
            }
            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(cachedData);

            return View(employees);
        }
        public async Task<List<Employee>> GetDummyEmployeesAsync()
        {
            List<Employee> employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Rajesh Kumar", Address = "123 Main St", Phone = "555-1234" },
    new Employee { Id = 2, Name = "Priya Sharma", Address = "456 Elm St", Phone = "555-5678" },
    new Employee { Id = 3, Name = "Amit Patel", Address = "789 Oak St", Phone = "555-9012" },
    new Employee { Id = 4, Name = "Ananya Singh", Address = "321 Pine St", Phone = "555-3456" },
    new Employee { Id = 5, Name = "Nikhil Gupta", Address = "654 Maple St", Phone = "555-7890" },
    new Employee { Id = 6, Name = "Sneha Sharma", Address = "987 Cedar St", Phone = "555-2345" },
    new Employee { Id = 7, Name = "Vikram Singh", Address = "654 Birch St", Phone = "555-6789" },
    new Employee { Id = 8, Name = "Deepika Verma", Address = "321 Spruce St", Phone = "555-0123" },
    new Employee { Id = 9, Name = "Rahul Kapoor", Address = "789 Walnut St", Phone = "555-4567" },
    new Employee { Id = 10, Name = "Anjali Sharma", Address = "123 Cherry St", Phone = "555-8901" }
};

            // Simulate an asynchronous operation, such as fetching data from a database or API
            await Task.Delay(100);

            return employees;
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
    }
}