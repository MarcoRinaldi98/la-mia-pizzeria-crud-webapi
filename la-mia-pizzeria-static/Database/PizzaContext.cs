using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static.Models;
using Azure;
using la_mia_pizzeria_static.Models.Database_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace la_mia_pizzeria_static.Database
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        // Liste dei dati nel Database
        public DbSet<Pizza> Pizze { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        // Collegamento al DataBase
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Pizzeria2023;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}
