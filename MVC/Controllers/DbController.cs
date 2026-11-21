using APP.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MVC.Controllers
{
    // Database Controller for seeding the database with initial data.
    public class DbController : Controller
    {
        [Route("SeedDb")] // changes the route of the action to /SeedDb
        public IActionResult Seed()
        {
            var dbFactory = new DbFactory();
            dbFactory.SeedDb();
            return Content("<h1>Database seed successful.</h1>", "text/html", Encoding.UTF8);
        }
    }
}