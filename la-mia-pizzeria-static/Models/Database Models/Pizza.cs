using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure;
using la_mia_pizzeria_static.Models.Database_Models;
using la_mia_pizzeria_static.ValidationAttributes;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        // ATTRIBUTI
        public int Id { get; set; }
        //[Url(ErrorMessage = "Devi inserire un link valido ad un'immagine")]
        [MaxLength(500, ErrorMessage = "Il link non può essere lungo più di 500 caratteri")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Il nome della pizza è obbligatorio")]
        [MaxLength(100, ErrorMessage = "La lunghezza massima del nome della pizza è di 100 caratteri")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descrizione dell'articolo è obbligatoria!")]
        [MaxLength(5000, ErrorMessage = "La lunghezza massima della descrizione della pizza è di 5000 caratteri")]
        [MoreThanFiveWords]
        public string Description { get; set; }
        [Required(ErrorMessage = "Il prezzo della pizza è obbligatorio")]
        [Range(1, 100, ErrorMessage = "Il prezzo dev'essere di almeno 1 euro e massimo di 100 euro")]
        public decimal Price { get; set; }

        // relazione 1:N
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // relazione N:N 
        public List<Ingredient>? Ingredients { get; set; }

        // COSTRUTTORE
        public Pizza()
        {
            
        }

        public Pizza(string image, string name, string description, decimal price) 
        {
            this.Image = image;
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }
    }
}
