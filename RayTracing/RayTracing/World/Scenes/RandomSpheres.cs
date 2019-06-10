using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util;
using RayTracing.Tracers;
using RayTracing.Lights;
using RayTracing.Cameras;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Composite;
using RayTracing.Materials;
using RayTracing.GeometricObjects.Primitives;

namespace RayTracing.World.Scenes
{
    class RandomSpheres : SceneBase
    {
        public override string Name
        {
            get { return "RandomSpheres"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 1;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(0.0, 0.0, 20),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          3600);

            Camera = pinhole;

            Directional l = new Directional();
            l.SetDirection(-10, 20, 20);
            l.ScaleRadiance = 3.0f;
            l.Shadows = false;
            AddLight(l);

            int numSpheres = 1000000;

            double volume = 0.1 / numSpheres;
            double radius = Math.Pow(0.75 * volume / Math.PI, 0.333333);

            Grid grid = new Grid();
            Rnd.SetRandSeed(15);

            for(int j = 0; j < numSpheres; j++)
            {
                Matte matte = new Matte();
                matte.SetKa(0.25f);
                matte.SetKd(0.75f);
                matte.SetColor(new Vec3(Rnd.RandDouble(),
                                        Rnd.RandDouble(),
                                        Rnd.RandDouble()));

                Sphere sphere = new Sphere();
                sphere.Radius = radius;
                sphere.SetCenter(1.0f - 2.0f * (float)Rnd.RandDouble(),
                                 1.0f - 2.0f * (float)Rnd.RandDouble(),
                                 1.0f - 2.0f * (float)Rnd.RandDouble());
                sphere.Material = matte;
                grid.AddObject(sphere);
            }

            grid.SetupCells();

            AddObject(grid);
        }
    }
}
