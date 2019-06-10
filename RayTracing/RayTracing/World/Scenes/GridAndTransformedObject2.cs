using RayTracing.Cameras;
using RayTracing.GeometricObjects;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Tracers;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.World.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.World.Scenes
{
    class GridAndTransformedObject2 : SceneBase
    {
        public override string Name
        {
            get { return "GridAndTransformedObject2"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(400, 400, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 1;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(10, 15, 30),
                                          new Vec3(0, 0, 0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          4000);

            Camera = pinhole;

            Directional l = new Directional();
            l.SetDirection(-10, 20, 20);
            l.ScaleRadiance = 3.0f;
            l.Shadows = true;
            
            AddLight(l);

            int numSpheres = 1000;

            double volume = 0.1 / numSpheres;
            float radius = (float)(10.0 * Math.Pow(0.75 * volume / Math.PI, 0.333333));

            Grid grid = new Grid();

            Rnd.SetRandSeed(15);

            for (int j = 0; j < numSpheres; j++)
            {
                Matte matte = new Matte();
                matte.SetKa(0.25f);
                matte.SetKd(0.75f);
                matte.SetColor(new Vec3(Rnd.RandDouble(),
                                        Rnd.RandDouble(),
                                        Rnd.RandDouble()));

                Sphere sphere = new Sphere();
                sphere.Material = matte;

                Instance instance = new Instance(sphere);
                instance.Scale(radius);
                instance.Translate(new Vec3(1.0 - 2.0 * Rnd.RandDouble(),
                                            1.0 - 2.0 * Rnd.RandDouble(),
                                            1.0 - 2.0 * Rnd.RandDouble()));
                instance.ComputeBoundingBox();

                grid.AddObject(instance);
            }

            grid.SetupCells();

            AddObject(grid);
        }
    }
}
