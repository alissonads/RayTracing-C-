using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Observer
{
    interface ISceneObservable
    {
        void RegisterObserver(ISceneObserver observer);

        void RemoveObserver(ISceneObserver observer);

        void RemoveObserver(int id);

        void NotifyObservers_RayTracerStarted();

        void NotifyObservers_RayTracerFinished();

        void NotifyObservers_DisplayPixel(int row, int column, Vec3 pixelColor);
        
    }
}
