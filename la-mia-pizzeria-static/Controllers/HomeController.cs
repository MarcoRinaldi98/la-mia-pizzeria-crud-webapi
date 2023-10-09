using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class HomeController : Controller
    {
        // Custom Logger
        private ICustomLogger _myLogger;
        // Collegamento al DataBase
        private PizzaContext _myDatabase;

        public HomeController(PizzaContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        // Vista Index
        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Home > Index");

            return View();
        }

        // Vista UtenteIndex
        public IActionResult UtenteIndex()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Home > UtenteIndex");

            List<Pizza> pizze = _myDatabase.Pizze.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList<Pizza>();

            return View(pizze);
        }

        // Vista Datails
        public IActionResult Details(int id)
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Home > Details");

            Pizza? foundedPizza = _myDatabase.Pizze.Where(Article => Article.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (foundedPizza == null)
            {
                return NotFound($"La pizza {id} non è stata trovata!");
            }
            else
            {
                return View("Details", foundedPizza);
            }
        }

        // Vista Privacy
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