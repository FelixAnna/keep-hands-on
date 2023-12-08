using System.Collections.Generic;

namespace MovieStudio.Movie
{
    public class MovieStatistics
    {
        public Dictionary<string, int> MovieGenres { get; }
        internal int TotalActors { get; private set; }
        internal int TotalCameramen { get; private set; }
        internal List<string> SuperActors { get; private set; }

        public MovieStatistics()
        {
            MovieGenres = new Dictionary<string, int>();
            TotalActors = 0;
            TotalCameramen = 0;
            SuperActors = new List<string>();
        }

        public void IncreaseActorsCount(int actorsCount)
        {
            TotalActors += actorsCount;
        }

        public void IncreaseCameramenCount(int cameramenCount)
        {
            TotalCameramen += cameramenCount;
        }

        public void AddSuperActors(List<string> superActors)
        {
            this.SuperActors.AddRange(superActors);
        }
    }
}
