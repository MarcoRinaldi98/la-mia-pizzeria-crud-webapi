namespace la_mia_pizzeria_static.Models.Database_Models
{
    public class Ingredient
    {
        // ATTRIBUTI
        public int Id { get; set; }
        public string Title { get; set; }

        // relazione N:N 
        public List<Pizza> Pizze { get; set; }

        // COSTRUTTORE
        public Ingredient()
        {

        }
    }
}
