using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;

namespace MovieStudio.Movie.Services
{
    public class MovieArchiveService : IMovieArchiveService
    {
        public List<Movie> MovieArchive { get; }

        public MovieArchiveService()
        {
            MovieArchive = new List<Movie>();
        }

        public virtual MovieStatistics LoadMovieDatabase(string fileName)
        {
            var movies = ReadArchivedMovie(fileName);

            try
            {
                AddMoviesToArchive(movies);
                return GetArchiveStatistics();
            }
            catch (IOException)
            {
                Console.WriteLine("Movie archive is damaged or empty");
                return new MovieStatistics();
            }

        }

        private List<Movie> ReadArchivedMovie(string filename)
        {
            var resource = ReadResourceFile(filename);

            var deserializer = new DeserializerBuilder().Build();

            var movie = deserializer.Deserialize<List<Movie>>(resource);

            return movie;
        }

        private string ReadResourceFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));


            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private MovieStatistics GetArchiveStatistics()
        {
            MovieStatistics movieStatistics = new();

            Console.WriteLine($"Movies in archive: {MovieArchive.Count}");
            MovieArchive.ForEach(movie =>
            {
                string currentMovieGenre = movie.GetGenre();
                movieStatistics.MovieGenres.Add(
                        currentMovieGenre,
                        movieStatistics.MovieGenres.FirstOrDefault(x => x.Key.Contains(currentMovieGenre)).Value);

            });
            MovieArchive.ForEach(movie =>
            {
                movieStatistics.IncreaseActorsCount(movie.GetActorCount());
                movieStatistics.IncreaseCameramenCount(movie.GetCameramenCount());
                movieStatistics.AddSuperActors(movie.GetSuperstars());
            });
            return movieStatistics;
        }

        private List<Movie> AddMoviesToArchive(List<Movie> movies)
        {
            MovieArchive.AddRange(movies);
            return MovieArchive;
        }

        public void Archive(Movie movie)
        {
            MovieArchive.Add(movie);
        }
    }
}
