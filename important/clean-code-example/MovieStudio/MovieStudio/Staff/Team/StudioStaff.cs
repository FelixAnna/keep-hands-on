using System.Collections.Generic;
using System.Linq;

namespace MovieStudio.Staff.Team
{
    public class StudioStaff
    {
        protected IEnumerable<Actor> actors;
        protected IEnumerable<CameraMan> cameramen;

        public StudioStaff(IEnumerable<Actor> actors, IEnumerable<CameraMan> cameramen)
        {
            this.actors = actors;
            this.cameramen = cameramen;
        }

        public IList<Actor> GetSuperActors()
        {
            return this.actors.Where(a => a is SuperActor).ToList();
        }

        public IList<Actor> GetActors()
        {
            return actors.ToList();
        }

        public IList<CameraMan> GetCameramen()
        {
            return cameramen.ToList();
        }
    }
}
