using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        // ATTRIBUTI
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo della categoria è obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il titolo della categoria non può superare i 50 caratteri")]
        public string Title { get; set; }

        // relazione 1:N 
        public List<Pizza>? Pizze { get; set; }

        // COSTRUTTORE
        public Category()
        {

        }
    }
}
