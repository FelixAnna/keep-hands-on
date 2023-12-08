using MovieStudio.Finance;
using MovieStudio.Movie;
using MovieStudio.Staff.Team;
using MovieStudio.Thirdparty;

namespace MovieStudio
{
    public class MovieDefinition
    {
        public long Budget { get; }
        public string MovieName { get; }
        public Genre MovieGenre { get; }
        public StudioStaff MovieStaff { get; }
        public int DaysInProduction { get; }


        public MovieDefinition(long budget, string movieName, Genre movieGenre,
                               StudioStaff movieStaffCollection, int daysInProduction)
        {
            this.Budget = budget;
            this.MovieName = movieName;
            this.MovieGenre = movieGenre;
            this.MovieStaff = movieStaffCollection;
            this.DaysInProduction = daysInProduction;
        }

        public Movie.Movie ToMovie()
        {
            return new Movie.Movie(MovieName, MovieGenre, MovieStaff, DaysInProduction);
        }

        public MovieBudget ToMovieBudget(long initialBudget)
        {
            return new MovieBudget(initialBudget + Budget);
        }
    }
}
