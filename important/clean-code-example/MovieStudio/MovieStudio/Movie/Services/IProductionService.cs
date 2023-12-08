using MovieStudio.Staff;
using System.Collections.Generic;

namespace MovieStudio.Movie.Services
{
    public interface IProductionService
    {
        bool LightsCameraAction(IEnumerable<Actor> actors, IEnumerable<CameraMan> cameramen);
    }
}