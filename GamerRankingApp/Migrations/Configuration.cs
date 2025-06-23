namespace GamerRankingApp.Migrations
{
    using GamerRankingApp.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GamerRankingApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GamerRankingApp.Models.ApplicationDbContext context)
        {
            //  Este método se llamará después de cada migración.
            //  Puedes usar el DbSet<T>.AddOrUpdate() método de extensión para evitar crear datos duplicados.

            // Semilla de Videojuegos (RETO 02)
            if (!context.Videojuegos.Any())
            {
                context.Videojuegos.AddOrUpdate(
                    v => v.Nombre, // Propiedad para identificar si un registro ya existe
                    new Videojuego { Nombre = "Dark Souls", Compania = "From Software", AnoLanzamiento = 2011, Precio = 39.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Sekiro: Shadows Die Twice", Compania = "From Software", AnoLanzamiento = 2019, Precio = 59.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Bloodborne", Compania = "From Software", AnoLanzamiento = 2015, Precio = 19.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Demon's Souls", Compania = "From Software", AnoLanzamiento = 2009, Precio = 39.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Cuphead", Compania = "StudioMDHR", AnoLanzamiento = 2017, Precio = 19.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Contra", Compania = "Konami", AnoLanzamiento = 1987, Precio = 6.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Nioh", Compania = "Koei Tecmo", AnoLanzamiento = 2017, Precio = 19.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Celeste", Compania = "Extremely OK Games", AnoLanzamiento = 2018, Precio = 7.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Battletoads", Compania = "Rare", AnoLanzamiento = 1991, Precio = 5.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Blasphemous", Compania = "Team17", AnoLanzamiento = 2019, Precio = 24.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Teenage Mutant Ninja Turtles", Compania = "Konami", AnoLanzamiento = 1989, Precio = 12.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Ninja Gaiden Black", Compania = "Koei Tecmo", AnoLanzamiento = 2005, Precio = 25.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Ghosts 'n Goblins", Compania = "Capcom", AnoLanzamiento = 1985, Precio = 6.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Salt and Sanctuary", Compania = "Ska Studios", AnoLanzamiento = 2016, Precio = 17.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Dark Souls III", Compania = "From Software", AnoLanzamiento = 2016, Precio = 59.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Super Meat Boy", Compania = "Direct2Drive", AnoLanzamiento = 2010, Precio = 5.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Dark Souls II", Compania = "From Software", AnoLanzamiento = 2014, Precio = 39.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Hollow Knight", Compania = "Team Cherry", AnoLanzamiento = 2017, Precio = 14.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Super Mario Maker 2", Compania = "Nintendo", AnoLanzamiento = 2019, Precio = 59.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" },
                    new Videojuego { Nombre = "Elden Ring", Compania = "From Software", AnoLanzamiento = 2022, Precio = 69.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Sistema" }
                );
                context.SaveChanges();
            }

            // Semilla de Roles y Usuarios (RETO 03, se detalla más adelante)
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Crear Roles si no existen
            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!roleManager.RoleExists("Auxiliar de Registro"))
            {
                roleManager.Create(new IdentityRole("Auxiliar de Registro"));
            }

            // Crear Usuario Administrador si no existe
            if (userManager.FindByName("admin@gamerranking.com") == null)
            {
                var adminUser = new ApplicationUser { UserName = "admin@gamerranking.com", Email = "admin@gamerranking.com" };
                var result = userManager.Create(adminUser, "AdminP@ssw0rd1!"); // Contraseña fuerte recomendada
                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Administrator");
                }
            }

            // Crear Usuario Auxiliar de Registro si no existe
            if (userManager.FindByName("auxiliar@gamerranking.com") == null)
            {
                var auxiliarUser = new ApplicationUser { UserName = "auxiliar@gamerranking.com", Email = "auxiliar@gamerranking.com" };
                var result = userManager.Create(auxiliarUser, "AuxiliarP@ssw0rd1!"); // Contraseña fuerte recomendada
                if (result.Succeeded)
                {
                    userManager.AddToRole(auxiliarUser.Id, "Auxiliar de Registro");
                }
            }

            context.SaveChanges();
        }
    }
}
