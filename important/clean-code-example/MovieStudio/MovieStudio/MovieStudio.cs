using MovieStudio.Finance;
using MovieStudio.Movie;
using MovieStudio.Movie.Services;
using MovieStudio.Staff;
using MovieStudio.Staff.Services;
using MovieStudio.Thirdparty.Exceptions;
using System;

namespace MovieStudio
{
    public class MovieStudio
    {
        private static readonly long s_initialBudget = 1000000L;
        private static readonly int s_potentialRisk = 15;

        private readonly IStaffingService _staffingService;
        private readonly IProductionService _productionService;
        private readonly IMovieArchiveService _movieArchiveService;

        public MovieStudio(IStaffingService staffingService, IProductionService productionService, IMovieArchiveService movieArchiveService)
        {
            _staffingService = staffingService;
            _productionService = productionService;
            _movieArchiveService = movieArchiveService;

            var movieStatistics = this._movieArchiveService.LoadMovieDatabase("film_archive.yaml");
            new MovieStatisticsPrinter(movieStatistics).Print();
        }

        public Movie.Movie CreateMovie(string recruiterName, string accountantName, MovieDefinition movieDefinition)
        {
            var movie = movieDefinition.ToMovie();
            var budget = movieDefinition.ToMovieBudget(s_initialBudget);

            this._staffingService.HireNewStaff(new Recruiter(recruiterName), new Accountant(accountantName));

            if (!CanBeProduced(budget.GetRemainingBudget(), movieDefinition.DaysInProduction))
            {
                throw new InsufficientBudgetException("Movie cannot be produced - budget is insufficient");
                
            }

            _staffingService.HireNewStaff(movieDefinition.MovieStaff);
            ProduceTheMovie(movie, budget);
            return movie;
        }

        private void ProduceTheMovie(Movie.Movie movie, MovieBudget budget)
        {
            try
            {
                MakeMovie(movie, budget);

                Console.WriteLine(budget.ToString());
                Console.WriteLine(_staffingService.ToString());

                _movieArchiveService.Archive(movie);
            }
            catch (BudgetIsOverException)
            {
                Console.WriteLine($"Movie production failed. Budget is over. Current progress is {movie.GetProgress()}");
            }
        }

        private void MakeMovie(Movie.Movie movie, MovieBudget budget)
        {
            while (movie.HasTime())
            {
                ProduceOneDay(movie);

                PaySalary(budget);
            }

            movie.FinishMaking();
        }

        private void PaySalary(MovieBudget budget)
        {
            var totalPaidAmount = this._staffingService.PayAllStaffs();
            budget.Decrease(totalPaidAmount);
        }

        private void ProduceOneDay(Movie.Movie movie)
        {
            var fullySuccessDay = this._productionService.LightsCameraAction(_staffingService.GetStaffByProfession<Actor>(), _staffingService.GetStaffByProfession<CameraMan>());
            movie.IncreaseProductionDays(fullySuccessDay);
        }

        private bool CanBeProduced(long proposedBudget, int daysInProduction)
        {
            long estimatedBudget = _staffingService.GetStaffEstimatedSalary(daysInProduction, s_potentialRisk);
            return proposedBudget >= estimatedBudget;
        }

    }
}
