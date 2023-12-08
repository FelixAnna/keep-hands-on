using System;

namespace MovieStudio.Movie
{
    public class MovieStatisticsPrinter
    {
        private readonly MovieStatistics movieStatistics;

        public MovieStatisticsPrinter(MovieStatistics movieStatistics)
        {
            this.movieStatistics = movieStatistics;
        }

        public void Print()
        {
            if (!IsEmpty())
            {
                Console.WriteLine($"Total: {movieStatistics.TotalActors} actors, {movieStatistics.TotalCameramen} cameramen, superstars: {string.Join(", ", movieStatistics.SuperActors)}\n");
            }
        }

        private bool IsEmpty()
        {
            return movieStatistics.TotalActors + movieStatistics.TotalCameramen + movieStatistics.SuperActors.Count > 0;
        }
    }
}
