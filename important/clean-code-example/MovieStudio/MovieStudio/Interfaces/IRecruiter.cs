using MovieStudio.Thirdparty;
using System;

namespace MovieStudio.Interfaces
{
    public interface IRecruiter
    {
        StudioEmployee Hire<T>(string name, Func<string, T> createInstance) where T : StudioEmployee;
    }
}
