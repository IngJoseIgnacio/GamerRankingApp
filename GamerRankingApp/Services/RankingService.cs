// Services/RankingService.cs
using GamerRankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // Necesario para .Include() si lo usas

namespace GamerRankingApp.Services
{
    public class RankingService
    {
        private readonly ApplicationDbContext _dbContext;

        // Inyección de dependencia del contexto de base de datos
        public RankingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RankingEntry> GenerateRanking(int topDesired)
        {
            // Consulta para calcular el puntaje promedio por videojuego
            var rankingQuery = _dbContext.Calificaciones
                .GroupBy(c => c.VideojuegoId)
                .Select(g => new
                {
                    VideojuegoId = g.Key,
                    AverageScore = g.Average(c => c.Puntuacion)
                })
                // Unir con la tabla Videojuegos para obtener el nombre y la compañía
                .Join(_dbContext.Videojuegos,
                      ranking => ranking.VideojuegoId,
                      videojuego => videojuego.Id,
                      (ranking, videojuego) => new
                      {
                          videojuego.Nombre,
                          videojuego.Compania,
                          ranking.AverageScore
                      })
                .OrderByDescending(r => r.AverageScore)
                .ToList(); // Materializar la consulta

            // Si topDesired es 0, significa que se desean todos los registros.
            // Si topDesired es > 0, se toma el número de registros especificado.
            var finalRankingData = (topDesired == 0) ? rankingQuery : rankingQuery.Take(topDesired).ToList();

            List<RankingEntry> rankingList = new List<RankingEntry>();
            int position = 0;
            // Calcular el "medio" basado en la cantidad REAL de elementos en el ranking final
            double halfOfCalculatedTop = (double)finalRankingData.Count / 2.0;

            foreach (var item in finalRankingData)
            {
                position++;
                string classification = (position <= Math.Ceiling(halfOfCalculatedTop)) ? "GOTY" : "AAA";

                rankingList.Add(new RankingEntry
                {
                    Nombre = item.Nombre,
                    Compania = item.Compania,
                    Puntaje = item.AverageScore,
                    Clasificacion = classification
                });
            }

            return rankingList;
        }
    }

    // Clase auxiliar para el resultado del ranking
    public class RankingEntry
    {
        public string Nombre { get; set; }
        public string Compania { get; set; }
        public decimal Puntaje { get; set; }
        public string Clasificacion { get; set; }
    }
}
