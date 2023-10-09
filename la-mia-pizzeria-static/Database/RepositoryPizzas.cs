using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Database_Models;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Database
{
    public class RepositoryPizzas : IRepositoryPizzas
    {
        private PizzaContext _db;

        public RepositoryPizzas(PizzaContext db)
        {
            _db = db;
        }

        public List<Pizza> GetPizzas()
        {
            List<Pizza> pizze = _db.Pizze.Include(pizza => pizza.Ingredients).ToList();
            return pizze;
        }

        public List<Pizza> GetPizzasByTitle(string title)
        {
            List<Pizza> foundedPizzas = _db.Pizze.Where(pizza => pizza.Name.ToLower().Contains(title.ToLower())).ToList();

            return foundedPizzas;
        }

        public Pizza GetPizzaById(int id)
        {
            Pizza? pizza = _db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizza != null)
            {
                return pizza;
            }
            else
            {
                throw new Exception("La pizza non è stata trovata!");
            }
        }

        public bool AddPizza(Pizza pizzaToAdd)
        {
            try
            {
                _db.Pizze.Add(pizzaToAdd);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ModifyPizza(int id, PizzaFormModel updatedPizza)
        {
            Pizza? pizzaToUpdate = _db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Image = updatedPizza.Pizza.Image;
                pizzaToUpdate.Name = updatedPizza.Pizza.Name;
                pizzaToUpdate.Description = updatedPizza.Pizza.Description;
                pizzaToUpdate.Price = updatedPizza.Pizza.Price;
                pizzaToUpdate.CategoryId = updatedPizza.Pizza.CategoryId;

                if (updatedPizza.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelectedId in updatedPizza.SelectedIngredientsId)
                    {
                        int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                        Ingredient? ingredientInDb = _db.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }

                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeletePizza(int id)
        {
            Pizza? pizzaToDelete = _db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete == null)
            {
                return false;
            }

            _db.Pizze.Remove(pizzaToDelete);
            _db.SaveChanges();

            return true;
        }
    }
}
