using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Database;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using la_mia_pizzeria_static.CustomLoggers;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using la_mia_pizzeria_static.Models.Database_Models;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "USER,ADMIN")]
    public class PizzaController : Controller
    {
        // Custom Logger
        private ICustomLogger _myLogger;
        // Collegamento al DataBase
        private PizzaContext _myDatabase;

        public PizzaController(PizzaContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        // Vista Index
        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Pizza > Index");

            List<Pizza> pizze = _myDatabase.Pizze.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList<Pizza>();

            return View(pizze);
        }

        // Vista Details
        public IActionResult Details(int id)
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Pizza > Details");

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

        // Creazione di una pizza
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Pizza > Create");

            List<Category> categories = _myDatabase.Categories.ToList();

            List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
            List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();

            foreach (Ingredient ingredient in databaseAllIngredients)
            {
                allIngredientsSelectList.Add(
                    new SelectListItem
                    {
                        Text = ingredient.Title,
                        Value = ingredient.Id.ToString()
                    });
            }

            PizzaFormModel model = new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = allIngredientsSelectList };

            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
                List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();

                foreach (Ingredient ingredient in databaseAllIngredients)
                {
                    allIngredientsSelectList.Add(
                        new SelectListItem
                        {
                            Text = ingredient.Title,
                            Value = ingredient.Id.ToString()
                        });
                }

                data.Ingredients = allIngredientsSelectList;

                return View("Create", data);
            }

            data.Pizza.Ingredients = new List<Ingredient>();

            if (data.SelectedIngredientsId != null)
            {
                foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                {
                    int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                    Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                    if (ingredientInDb != null)
                    {
                        data.Pizza.Ingredients.Add(ingredientInDb);
                    }
                }
            }

            _myDatabase.Pizze.Add(data.Pizza);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
        }

        // Modifica di una pizza
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            Pizza? pizzaToEdit = _myDatabase.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToEdit == null)
            {
                return NotFound("La pizza che vuoi modificare non è stata trovata");
            }
            else
            {
                _myLogger.WriteLog("L'utente è arrivato sulla pagina Pizza > Update");

                List<Category> categories = _myDatabase.Categories.ToList();

                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> selectListItem = new List<SelectListItem>();

                foreach (Ingredient ingredient in dbIngredientList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = ingredient.Id.ToString(),
                        Text = ingredient.Title,
                        Selected = pizzaToEdit.Ingredients.Any(ingredientAssociated => ingredientAssociated.Id == ingredient.Id)
                    });
                }

                PizzaFormModel model = new PizzaFormModel { Pizza = pizzaToEdit, Categories = categories, Ingredients = selectListItem };

                return View("Update", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> selectListItem = new List<SelectListItem>();

                foreach (Ingredient ingredient in dbIngredientList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = ingredient.Id.ToString(),
                        Text = ingredient.Title
                    });
                }

                data.Ingredients = selectListItem;

                return View("Update", data);
            }

            Pizza? pizzaToUpdate = _myDatabase.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Image = data.Pizza.Image;
                pizzaToUpdate.Name = data.Pizza.Name;
                pizzaToUpdate.Description = data.Pizza.Description;
                pizzaToUpdate.Price = data.Pizza.Price;
                pizzaToUpdate.CategoryId = data.Pizza.CategoryId;


                if (data.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                    {
                        int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                        Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace non è stata trovata la pizza da aggiornare");
            }
        }

        // Cancellazione di una pizza
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizze.Remove(pizzaToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La pizza da eliminare non è stata trovata!");
            }
        }
    }
}
