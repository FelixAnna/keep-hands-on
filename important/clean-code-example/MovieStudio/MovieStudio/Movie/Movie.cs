using MovieStudio.Staff;
using MovieStudio.Staff.Team;
using MovieStudio.Thirdparty;
using System.Collections.Generic;

namespace MovieStudio.Movie
{
    public class Movie
    {
        public string Name { get; private set; }
        public Genre Genre { get; private set; }
        public int DaysInProduction { get; private set; }
        public Dictionary<string, int> Crew { get; private set; }
        public List<string> Superstars { get; private set; }

        public bool IsFinished { get; private set; }
        public MovieProductionSchedule movieProductionSchedule { get; private set; }

        public Movie() { }

        public Movie(string name, Genre genre, StudioStaff staff, int daysInProduction)
        {
            this.Name = name;
            this.Genre = genre;
            this.IsFinished = false;
            this.DaysInProduction = 0;
            this.movieProductionSchedule = new MovieProductionSchedule(daysInProduction);
            this.SetCrewAndSuperStars(staff);
        }

        private void SetCrewAndSuperStars(StudioStaff staff)
        {
            SetCrew(staff);
            SetSuperStars(staff);
        }

        private void SetCrew(StudioStaff staff)
        {
            this.Crew = new Dictionary<string, int>();

            Crew.Add(nameof(Actor).ToLower(), staff.GetActors().Count);
            Crew.Add(nameof(CameraMan).ToLower(), staff.GetCameramen().Count);
        }

        private void SetSuperStars(StudioStaff staff)
        {
            this.Superstars = new List<string>();

            foreach (Actor actor in staff.GetSuperActors())
            {
                Superstars.Add(actor.Name);
            }
        }

        public int GetActorCount()
        {
            var key = nameof(Actor).ToLower();
            if (!Crew.ContainsKey(key))
            {
                return 0;
            }

            return Crew[key];
        }

        public int GetCameramenCount()
        {
            var key = nameof(CameraMan).ToLower();
            if (!Crew.ContainsKey(key))
            {
                return 0;
            }

            return Crew[key];
        }
        public void IncreaseProductionDays(bool fullySuccess)
        {
            this.DaysInProduction++;
            if (fullySuccess)
            {
                movieProductionSchedule.MoveOn();
            }
        }

        public void FinishMaking()
        {
            this.IsFinished = true;
        }

        public override string ToString()
        {
            return string.Format($"Movie {Name} [{Genre}], status: {(IsFinished ? "finished" : "in production")}, days in production:{DaysInProduction}");
        }

        public string GetGenre()
        {
            return Genre.ToString();
        }

        public List<string> GetSuperstars()
        {
            return Superstars;
        }


        public bool HasTime()
        {
            return movieProductionSchedule.HasTime();
        }

        public double GetProgress()
        {
            return movieProductionSchedule.GetProgress();
        }
    }
}
