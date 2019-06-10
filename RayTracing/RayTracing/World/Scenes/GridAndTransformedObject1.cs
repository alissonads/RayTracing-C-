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
    class GridAndTransformedObject1 : SceneBase
    {
        public override string Name
        {
            get { return "GridAndTransformedObject1"; }
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

            Pinhole pinhole = new Pinhole(new Vec3(),
                                          new Vec3(-100, 0, 0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          400);

            Camera = pinhole;

            Directional l1 = new Directional();
            l1.SetDirection(1, 0, 0);
            l1.ScaleRadiance = 1.0f;
            l1.Shadows = true;

            Directional l2 = new Directional();
            l2.SetDirection(0, 1, 0);
            l2.ScaleRadiance = 2.0f;
            l2.Shadows = true;

            Directional l3 = new Directional();
            l3.SetDirection(0, 0, 1);
            l3.ScaleRadiance = 1.5f;
            l3.Shadows = true;

            Directional l4 = new Directional();
            l4.SetDirection(-1, 0, 0);
            l4.ScaleRadiance = 1.0f;
            l4.Shadows = true;

            Directional l5 = new Directional();
            l5.SetDirection(0, -1, 0);
            l5.ScaleRadiance = 1.5f;
            l5.Shadows = true;

            Directional l6 = new Directional();
            l6.SetDirection(0, 0, -1);
            l6.ScaleRadiance = 1.5f;
            l6.Shadows = true;

            AddLight(l1);
            AddLight(l2);
            AddLight(l3);
            AddLight(l4);
            AddLight(l5);
            AddLight(l6);

            int numSpheres = 25;

            double volume = 0.1 / numSpheres;
            float radius = (float)(2.5 * Math.Pow(0.75 * volume / Math.PI, 0.333333));

            Grid grid = new Grid();
            Rnd.SetRandSeed(14);

            for (int j = 0; j < numSpheres; j++)
            {
                Matte matte = new Matte();
                matte.SetKa(0.25f);
                matte.SetKd(0.85f);
                matte.SetColor(new Vec3(Rnd.RandDouble(),
                                        Rnd.RandDouble(),
                                        Rnd.RandDouble()));

                Sphere sphere = new Sphere();
                sphere.Material = matte;

                Instance instance = new Instance(sphere);
                instance.Scale(radius, radius, radius);
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
