namespace MovieStudio.Movie
{
    public class MovieProductionSchedule
    {
        private readonly int plannedDaysInProduction;
        private int actualDaysInProduction;

        public MovieProductionSchedule(int daysInProduction)
        {
            plannedDaysInProduction = daysInProduction;
            this.actualDaysInProduction = daysInProduction;
        }

        public void MoveOn()
        {
            actualDaysInProduction--;
        }

        public bool HasTime()
        {
            return actualDaysInProduction > 0;
        }

        public double GetProgress()
        {
            return (1 - actualDaysInProduction / plannedDaysInProduction) * 100;
        }
    }
}
