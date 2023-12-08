using MovieStudio.Staff;
using System.Collections.Generic;
using System.Linq;

namespace MovieStudio.Movie.Services
{
    public class ProductionService : IProductionService
    {
        public bool LightsCameraAction(IEnumerable<Actor> actors, IEnumerable<CameraMan> cameramen)
        {
            var allActorsPerformed = actors.All(actor => actor.Act());
            var allCameraMenPerformed = cameramen.All(cameraman => cameraman.Shoot());

            return allActorsPerformed && allCameraMenPerformed;
        }

    }
}
