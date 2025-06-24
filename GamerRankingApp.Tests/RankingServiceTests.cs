// GamerRankingApp.Tests/RankingServiceTests.cs
using GamerRankingApp.Models; // Para acceder a las entidades
using GamerRankingApp.Services; // Para acceder al servicio
using Moq; // Para simular dependencias
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity; // Para DbContext y simulaciones de DbSet
using System.Linq;

namespace GamerRankingApp.Tests
{
    [TestFixture] // Atributo de NUnit para marcar una clase de pruebas
    public class RankingServiceTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private RankingService _rankingService;

        [SetUp] // Se ejecuta antes de cada prueba
        public void Setup()
        {
            // Configuración de un DbContext simulado
            _mockDbContext = new Mock<ApplicationDbContext>();

            // Datos de prueba para Videojuegos
            var videojuegos = new List<Videojuego>
                {
                    new Videojuego { Id = 1, Nombre = "Dark Souls", Compania = "From Software", AnoLanzamiento = 2011, Precio = 39.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Videojuego { Id = 2, Nombre = "Sekiro", Compania = "From Software", AnoLanzamiento = 2019, Precio = 59.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Videojuego { Id = 3, Nombre = "Cuphead", Compania = "StudioMDHR", AnoLanzamiento = 2017, Precio = 19.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Videojuego { Id = 4, Nombre = "Elden Ring", Compania = "From Software", AnoLanzamiento = 2022, Precio = 69.99m, Puntaje = 0.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" }
                }.AsQueryable();

            // Datos de prueba para Calificaciones
            var calificaciones = new List<Calificacion>
                {
                    // Dark Souls (Id: 1)
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 1, NicknameJugador = "Player1", Puntuacion = 4.50m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 1, NicknameJugador = "Player2", Puntuacion = 4.80m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 1, NicknameJugador = "Player3", Puntuacion = 4.70m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },

                    // Sekiro (Id: 2)
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 2, NicknameJugador = "Player4", Puntuacion = 4.90m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 2, NicknameJugador = "Player5", Puntuacion = 5.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },

                    // Cuphead (Id: 3)
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 3, NicknameJugador = "Player6", Puntuacion = 4.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 3, NicknameJugador = "Player7", Puntuacion = 3.90m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },

                    // Elden Ring (Id: 4)
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 4, NicknameJugador = "Player8", Puntuacion = 4.95m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 4, NicknameJugador = "Player9", Puntuacion = 5.00m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" },
                    new Calificacion { Id = Guid.NewGuid(), VideojuegoId = 4, NicknameJugador = "Player10", Puntuacion = 4.90m, FechaActualizacion = DateTime.Now, UsuarioActualizacion = "Test" }

                }.AsQueryable();

            // Configurar el mock de DbSet para Videojuegos
            var mockSetVideojuegos = new Mock<DbSet<Videojuego>>();
            mockSetVideojuegos.As<IQueryable<Videojuego>>().Setup(m => m.Provider).Returns(videojuegos.Provider);
            mockSetVideojuegos.As<IQueryable<Videojuego>>().Setup(m => m.Expression).Returns(videojuegos.Expression);
            mockSetVideojuegos.As<IQueryable<Videojuego>>().Setup(m => m.ElementType).Returns(videojuegos.ElementType);
            mockSetVideojuegos.As<IQueryable<Videojuego>>().Setup(m => m.GetEnumerator()).Returns(videojuegos.GetEnumerator());
            _mockDbContext.Setup(c => c.Videojuegos).Returns(mockSetVideojuegos.Object);

            // Configurar el mock de DbSet para Calificaciones
            var mockSetCalificaciones = new Mock<DbSet<Calificacion>>();
            mockSetCalificaciones.As<IQueryable<Calificacion>>().Setup(m => m.Provider).Returns(calificaciones.Provider);
            mockSetCalificaciones.As<IQueryable<Calificacion>>().Setup(m => m.Expression).Returns(calificaciones.Expression);
            mockSetCalificaciones.As<IQueryable<Calificacion>>().Setup(m => m.ElementType).Returns(calificaciones.ElementType);
            mockSetCalificaciones.As<IQueryable<Calificacion>>().Setup(m => m.GetEnumerator()).Returns(calificaciones.GetEnumerator());
            _mockDbContext.Setup(c => c.Calificaciones).Returns(mockSetCalificaciones.Object);

            // Instanciar el servicio con el DbContext simulado
            _rankingService = new RankingService(_mockDbContext.Object);
        }

        [Test] // Atributo de NUnit para marcar un método de prueba
        public void GenerateRanking_ShouldReturnCorrectAverageScores()
        {
            // Act
            var result = _rankingService.GenerateRanking(0); // Top 0 para todos los videojuegos

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count); // Debería haber 4 videojuegos en el ranking

            // Verificar puntajes promedio esperados (aproximados)
            Assert.AreEqual(4.97m, result.First(r => r.Nombre == "Elden Ring").Puntaje); // (4.95 + 5.00 + 4.90) / 3 = 4.95
            Assert.AreEqual(4.95m, result.First(r => r.Nombre == "Sekiro").Puntaje);    // (4.90 + 5.00) / 2 = 4.95
            Assert.AreEqual(4.67m, result.First(r => r.Nombre == "Dark Souls").Puntaje); // (4.50 + 4.80 + 4.70) / 3 = 4.666...
            Assert.AreEqual(3.95m, result.First(r => r.Nombre == "Cuphead").Puntaje);   // (4.00 + 3.90) / 2 = 3.95

            // Verificar orden descendente por puntaje
            Assert.AreEqual("Elden Ring", result[0].Nombre);
            Assert.AreEqual("Sekiro", result[1].Nombre);
            Assert.AreEqual("Dark Souls", result[2].Nombre);
            Assert.AreEqual("Cuphead", result[3].Nombre);
        }

        [Test]
        public void GenerateRanking_ShouldApplyCorrectClassification_GOTY_AAA()
        {
            // Act
            var resultTop4 = _rankingService.GenerateRanking(4); // Top 4

            // Assert para Top 4: mitad es 2 (ceil(4/2)=2)
            Assert.AreEqual("GOTY", resultTop4.First(r => r.Nombre == "Elden Ring").Clasificacion);
            Assert.AreEqual("GOTY", resultTop4.First(r => r.Nombre == "Sekiro").Clasificacion);
            Assert.AreEqual("AAA", resultTop4.First(r => r.Nombre == "Dark Souls").Clasificacion);
            Assert.AreEqual("AAA", resultTop4.First(r => r.Nombre == "Cuphead").Clasificacion);

            var resultTop3 = _rankingService.GenerateRanking(3); // Top 3
                                                                 // Assert para Top 3: mitad es 1.5, ceil(1.5)=2. Los 2 primeros son GOTY
            Assert.AreEqual("GOTY", resultTop3.First(r => r.Nombre == "Elden Ring").Clasificacion);
            Assert.AreEqual("GOTY", resultTop3.First(r => r.Nombre == "Sekiro").Clasificacion);
            Assert.AreEqual("AAA", resultTop3.First(r => r.Nombre == "Dark Souls").Clasificacion);

            var resultTop2 = _rankingService.GenerateRanking(2); // Top 2
                                                                 // Assert para Top 2: mitad es 1, ceil(2/2)=1. El primer es GOTY
            Assert.AreEqual("GOTY", resultTop2.First(r => r.Nombre == "Elden Ring").Clasificacion);
            Assert.AreEqual("AAA", resultTop2.First(r => r.Nombre == "Sekiro").Clasificacion);
        }

        [Test]
        public void GenerateRanking_ShouldReturnEmptyList_WhenNoCalificaciones()
        {
            // Configurar DbContext simulado sin calificaciones
            var emptyCalificaciones = new List<Calificacion>().AsQueryable();
            var mockSetCalificacionesEmpty = new Mock<DbSet<Calificacion>>();
            mockSetCalificacionesEmpty.As<IQueryable<Calificacion>>().Setup(m => m.Provider).Returns(emptyCalificaciones.Provider);
            mockSetCalificacionesEmpty.As<IQueryable<Calificacion>>().Setup(m => m.Expression).Returns(emptyCalificaciones.Expression);
            mockSetCalificacionesEmpty.As<IQueryable<Calificacion>>().Setup(m => m.ElementType).Returns(emptyCalificaciones.ElementType);
            mockSetCalificacionesEmpty.As<IQueryable<Calificacion>>().Setup(m => m.GetEnumerator()).Returns(emptyCalificaciones.GetEnumerator());
            _mockDbContext.Setup(c => c.Calificaciones).Returns(mockSetCalificacionesEmpty.Object);

            var emptyRankingService = new RankingService(_mockDbContext.Object);

            // Act
            var result = emptyRankingService.GenerateRanking(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GenerateRanking_ShouldRespectTopParameter()
        {
            // Act
            var resultTop2 = _rankingService.GenerateRanking(2);

            // Assert
            Assert.AreEqual(2, resultTop2.Count);
            Assert.AreEqual("Elden Ring", resultTop2[0].Nombre);
            Assert.AreEqual("Sekiro", resultTop2[1].Nombre);

            // Act
            var resultTop1 = _rankingService.GenerateRanking(1);

            // Assert
            Assert.AreEqual(1, resultTop1.Count);
            Assert.AreEqual("Elden Ring", resultTop1[0].Nombre);
        }

        [Test]
        public void GenerateRanking_ShouldHandleTopZeroAsAllEntries()
        {
            // Act
            var result = _rankingService.GenerateRanking(0);

            // Assert
            Assert.AreEqual(4, result.Count); // Debería devolver todos los videojuegos de prueba
        }
    }

    [TestFixture]
    public class VideojuegoModelTests
    {
        [Test]
        public void Videojuego_RequiredFields_ShouldBeInvalidWithoutValues()
        {
            // Arrange
            var videojuego = new Videojuego(); // No se establecen campos requeridos

            // Act
            var validationContext = new ValidationContext(videojuego);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(videojuego, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "El nombre del videojuego es obligatorio."));
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "La compañía es obligatoria."));
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "El año de lanzamiento es obligatorio."));
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "El precio es obligatorio."));
            // Puntaje tiene un valor por defecto, así que no debería haber error para este
        }

        [Test]
        public void Videojuego_AnoLanzamiento_ShouldBeInvalidOutsideRange()
        {
            // Arrange
            var videojuego = new Videojuego
            {
                Nombre = "Test",
                Compania = "Test",
                Precio = 10m,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacion = "TestUser",
                AnoLanzamiento = 1899 // Fuera de rango
            };

            // Act
            var validationContext = new ValidationContext(videojuego);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(videojuego, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "El año de lanzamiento debe ser entre 1900 y 2100."));

            videojuego.AnoLanzamiento = 2101; // Fuera de rango
            validationResults.Clear();
            isValid = Validator.TryValidateObject(videojuego, validationContext, validationResults, true);
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "El año de lanzamiento debe ser entre 1900 y 2100."));
        }

        [Test]
        public void Calificacion_Puntuacion_ShouldBeInvalidOutsideRange()
        {
            // Arrange
            var calificacion = new Calificacion
            {
                Id = Guid.NewGuid(),
                NicknameJugador = "Player",
                VideojuegoId = 1,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacion = "TestUser",
                Puntuacion = 5.01m // Fuera de rango
            };

            // Act
            var validationContext = new ValidationContext(calificacion);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(calificacion, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "La puntuación debe ser entre 0 y 5."));

            calificacion.Puntuacion = -0.01m; // Fuera de rango
            validationResults.Clear();
            isValid = Validator.TryValidateObject(calificacion, validationContext, validationResults, true);
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(r => r.ErrorMessage == "La puntuación debe ser entre 0 y 5."));
        }
    }
}
