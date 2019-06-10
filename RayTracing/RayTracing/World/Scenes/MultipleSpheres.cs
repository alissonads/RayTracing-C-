using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Util.Math;
using RayTracing.Tracers;

namespace RayTracing.World.Scenes
{
    class MultipleSpheres : SceneBase
    {
        public MultipleSpheres() : base()
        {}

        public override string Name
        {
            get { return "MultipleSpheres"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(200, 200, SystemOfCoordinates.SSC_INT);
            backgroundColor = ColorUtils.BLACK;
            tracer = new MultipleObjects(this);

            Sphere sphere = new Sphere(new Vec3(0.0, -25.0, 0.0), 80.0);
            sphere.Color = new Vec3(1.0, 0.0, 0.0);
            AddObject(sphere);

            sphere = new Sphere(new Vec3(0.0, 30.0, 0.0), 60.0);
            sphere.Color = new Vec3(1.0, 1.0, 0.0);
            AddObject(sphere);

            //Plane plane = new Plane(new Vec3(0.0), new Vec3(0.0, 1.0, 1.0));
            //plane.Color = new Vec3(0.0, 0.3, 0.0);
            //AddObject(plane);
        }

        public override void RenderScene()
        {
            Vec3 pixelColor = ColorUtils.BLACK;
            Ray ray = new Ray();
            double zw = -100.0;
            double x, y;

            //OpenWindow(vp.Hres, vp.Vres);
            NotifyObservers_RayTracerStarted();
            ray.D.Set(0.0, 0.0, 1.0);

            for (int r = 0; r < vp.Hres; r++)
            {
                for (int c = 0; c < vp.Vres; c++)
                {
                    x = vp.PixelSize.X * (c - 0.5 * (vp.Hres - 1.0));
                    y = vp.PixelSize.Y * (r - 0.5 * (vp.Vres - 1.0));
                    ray.O.Set(x, y, zw);
                    pixelColor = tracer.TraceRay(ray);

                    if (vp.Gama != 1.0)
                        pixelColor = ColorUtils.Powc(pixelColor, vp.InvGama);

                    DisplayPixel(r, c, pixelColor);
                }
            }
        }
    }
}
