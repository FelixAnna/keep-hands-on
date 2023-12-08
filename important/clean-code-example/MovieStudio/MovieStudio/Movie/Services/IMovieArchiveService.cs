using System.Collections.Generic;

namespace MovieStudio.Movie.Services
{
    public interface IMovieArchiveService
    {
        List<Movie> MovieArchive { get; }

        void Archive(Movie movie);
        MovieStatistics LoadMovieDatabase(string fileName);
    }
}