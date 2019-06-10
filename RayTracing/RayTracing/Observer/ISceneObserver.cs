using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;

namespace RayTracing.Observer
{
    interface ISceneObserver
    {
        void RayTracerStarted(int hres, int vres);

        void DisplayPixel(int x, int y, int rgb);

        void RayTracerFinished(ISceneObservable world);
    }
}
