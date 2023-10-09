using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Database
{
    public interface IRepositoryPizzas
    {
        public List<Pizza> GetPizzas();
        public List<Pizza> GetPizzasByTitle(string title);
        public Pizza GetPizzaById(int id);
        public bool AddPizza(Pizza pizzaToAdd);
        public bool ModifyPizza(int id, PizzaFormModel updatedPizza);
        public bool DeletePizza(int id);
    }
}
