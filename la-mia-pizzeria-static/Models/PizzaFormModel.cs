using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models.Database_Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }

        public List<SelectListItem>? Ingredients { get; set; }
        public List<string>? SelectedIngredientsId { get; set; }

        public void AddIngredientsToPizza(Pizza pizzaToUpdate, PizzaContext db)
        {
            if (SelectedIngredientsId != null)
            {
                foreach (string ingredientSelectedId in SelectedIngredientsId)
                {
                    if (int.TryParse(ingredientSelectedId, out int intIngredientSelectedId))
                    {
                        Ingredient ingredientInDb = db.Ingredients.FirstOrDefault(ingredient => ingredient.Id == intIngredientSelectedId);

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }
            }
        }
    }
}
