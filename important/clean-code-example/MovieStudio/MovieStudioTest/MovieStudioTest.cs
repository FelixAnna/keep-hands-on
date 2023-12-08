using MovieStudio;
using MovieStudio.Movie;
using MovieStudio.Movie.Services;
using MovieStudio.Staff;
using MovieStudio.Staff.Services;
using MovieStudio.Staff.Team;
using MovieStudio.Thirdparty;
using MovieStudio.Thirdparty.Exceptions;
using Xunit;

namespace MovieStudioTest
{
    public class MovieStudioTest
    {
        static MovieStudio.MovieStudio movieStudio;
        static string recruiterForTest = "Andrew Carnegie";
        static string accountantForTest = "William Welch Deloitte";

        public MovieStudioTest()
        {
            movieStudio = new MovieStudio.MovieStudio(new StaffingService(), new ProductionService(), new MovieArchiveService());
        }

        [Fact]
        public void CreateMovie_Titanic_WithValidMovieDefinition_ReturnsTrue_OnSuccessfulCompletion()
        {
            int PRODUCTION_SCHEDULE = 80;
            StudioStaff staff = new StudioStaff(
                    new Actor[]{
                        new SuperActor("Leo DiCaprio"), new SuperActor("Kate Winslet"),
                        new Actor("Billy Zane"), new Actor("Kathy Bates"),
                        new Actor("Frances Fisher"), new Actor("Bernard Hill"),
                        new Actor("Jonathan Hyde"), new Actor("Danny Nucci"),
                        new Actor("David Warner"), new Actor("Bill Paxton")
                    },
                    new CameraMan[]{
                        new CameraMan("Guy Norman Bee"),
                        new CameraMan("Marcis Cole"),
                        new CameraMan("Tony Guerin")
                    }
            );
            long budget = 1600000000L;
            MovieDefinition titanicMovie = new MovieDefinition(budget, "Titanic", Genre.DRAMA,
                    staff, PRODUCTION_SCHEDULE);
            Movie movie = movieStudio.CreateMovie(recruiterForTest, accountantForTest, titanicMovie);

            Assert.True(movie.IsFinished);
        }

        [Fact]
        public void CreateMovie_StarWars_WithValidMovieDefinition_ReturnsTrue_OnSuccessfulCompletion()
        {
            int PRODUCTION_SCHEDULE = 80;
            StudioStaff staff = new StudioStaff(
                    new Actor[]{
                        new SuperActor("Mark Hamill"), new SuperActor("Harrison Ford"),
                        new Actor("Carrie Fischer"), new Actor("Billy Dee Williams"),
                        new Actor("Anthony Daniels"), new Actor("David Prowse"),
                        new Actor("Peter Mayhew")
                    },
                    new CameraMan[]{
                        new CameraMan("John Campbell"),
                        new CameraMan("Bill Neil")
                    }
            );
            long budget = 6000000000L;
            MovieDefinition starWars3Movie = new MovieDefinition(budget, "Star Wars: Episode VI ?Return of the Jedi",
                    Genre.SCIFI, staff, PRODUCTION_SCHEDULE);
            Movie movie = movieStudio.CreateMovie(recruiterForTest, accountantForTest, starWars3Movie);
            Assert.True(movie.IsFinished);
        }

        [Fact]
        public void CreateMovie_ShouldCreateEmptyMovie_WithEmptyMovieDefinition_ReturnsTrue()
        {
            int PRODUCTION_SCHEDULE = 1;
            StudioStaff staff = new StudioStaff(
                    new Actor[] { },
                    new CameraMan[] { }
            );
            long budget = 1L;
            MovieDefinition emptyMovie = new MovieDefinition(budget, "Noname", Genre.COMEDY,
                    staff, PRODUCTION_SCHEDULE);
            Movie movie = movieStudio.CreateMovie(recruiterForTest, accountantForTest, emptyMovie);
            Assert.True(movie.IsFinished);
        }


        [Fact]
        public void CreateMovie_WhenBudgetExceeds_ReturnsFalse_OnUnsuccessfulCompletion()
        {
            int PRODUCTION_SCHEDULE = 200;
            StudioStaff staff = new StudioStaff(
                    new Actor[]{
                        new Actor("Taylor Kitsch"),
                        new Actor("Lynn Collins"),
                        new Actor("Samantha Morton"),
                        new SuperActor("Mark Strong"),
                        new Actor("Ciarán Hinds"),
                        new Actor("Dominic West"),
                        new Actor("James Purefoy"),
                        new SuperActor("Willem Dafoe")
                    },
                    new CameraMan[]{
                        new CameraMan("Carver Christians"),
                        new CameraMan("Scott Bourke"),
                        new CameraMan("Quentin Herriot"),
                        new CameraMan("Brandon Wyman")
                    }
            );
            long budget = 100000000L;
            MovieDefinition johnCarterMovie = new MovieDefinition(budget, "John Carter", Genre.FANTASY,
                    staff, PRODUCTION_SCHEDULE);
            Movie movie = movieStudio.CreateMovie(recruiterForTest, accountantForTest, johnCarterMovie);
            Assert.False(movie.IsFinished);
        }

        [Fact]
        public void CreateMovie_WhenInsufficientBudget_Throws_InsufficientBudgetException()
        {
            var message = "Movie cannot be produced - budget is insufficient";

            int PRODUCTION_SCHEDULE = 250;
            StudioStaff staff = new StudioStaff(
                    new Actor[]{
                        new SuperActor ("Channing Tatum"),
                        new SuperActor ("Taylor Kitsch"),
                        new SuperActor ("Keanu Reeves"),
                        new SuperActor ("Josh Holloway"),
                        new SuperActor ("Léa Seydoux"),
                        new SuperActor ("Hugh Jackman"),
                        new Actor("Rebecca Ferguson"),
                        new SuperActor ("Abbey Leee")
                    },
                    new CameraMan[]{
                        new CameraMan("Carver Christians"),
                        new CameraMan("Scott Bourke"),
                        new CameraMan("Quentin Herriot"),
                        new CameraMan("Brandon Wyman")
                    }
            );
            long budget = 100000000L;
            MovieDefinition johnCarterMovie = new MovieDefinition(budget, "Gambit", Genre.FANTASY,
                    staff, PRODUCTION_SCHEDULE);
            var exception = Assert.Throws<InsufficientBudgetException>(() => movieStudio.CreateMovie(recruiterForTest, accountantForTest, johnCarterMovie));
            Assert.Equal(message, exception.Message);

        }
    }
}
