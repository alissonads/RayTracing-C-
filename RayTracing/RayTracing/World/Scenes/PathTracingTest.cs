using RayTracing.Cameras;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Samplers;
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
    class PathTracingTest : SceneBase
    {
        public override string Name
        {
            get { return "PathTracingTest"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 100;
            vp.MaxDepth = 5;

            backgroundColor = ColorUtils.BLACK;
            tracer = new PathTrace(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 0.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(100, 45, 100),
                                          new Vec3(-10, 40, 0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          800, 1.5f);
            Camera = pinhole;

            Emissive emissive = new Emissive(ColorUtils.WHITE, 1.5f);

            ConcaveSphere sphere = new ConcaveSphere();
            sphere.Radius = 1000000;
            sphere.Shadows = false;
            sphere.Material = emissive;
            AddObject(sphere);

            float ka = 0.2f;

            Matte m1 = new Matte();
            m1.SetKa(ka);
            m1.SetKd(0.60f);
            m1.SetColor(ColorUtils.WHITE);
            m1.SetSampler(new MultiJittered(vp.NumSamples));

            Sphere largeSphere = new Sphere(new Vec3(38, 20, -24), 20);
            largeSphere.Material = m1;
            AddObject(largeSphere);

            Matte m2 = new Matte();
            m2.SetKa(ka);
            m2.SetKd(0.5f);
            m2.SetColor(0.85f);
            m2.SetSampler(new MultiJittered(vp.NumSamples));

            Sphere smallSphere = new Sphere(new Vec3(34, 12, 13), 12);
            smallSphere.Material = m2;
            AddObject(smallSphere);

            Matte m3 = new Matte();
            m3.SetKa(ka);
            m3.SetKd(0.75f);
            m3.SetColor(0.73f, 0.22f, 0.0f);
            m3.SetSampler(new MultiJittered(vp.NumSamples));

            Sphere mediumSphere = new Sphere(new Vec3(-7, 15, 42), 16);
            mediumSphere.Material = m3;
            AddObject(mediumSphere);

            Matte m4 = new Matte();
            m4.SetKa(ka);
            m4.SetKd(0.75f);
            m4.SetColor(0.60f);
            m4.SetSampler(new MultiJittered(vp.NumSamples));

            float b = 0;
            float t = 85;
            float r = 22;

            SolidCylinder cylinder = new SolidCylinder(b, t, r, m4);
            AddObject(cylinder);

            Matte m5 = new Matte();
            m5.SetKa(ka);
            m5.SetKd(0.75f);
            m5.SetColor(0.95f);
            m5.SetSampler(new MultiJittered(vp.NumSamples));

            Box box = new Box(new Vec3(-55, 0, -110),
                              new Vec3(-25, 60, 65),
                              m5);
            AddObject(box);

            Matte m6 = new Matte();
            m6.SetKa(0.15f);
            m6.SetKd(0.95f);
            m6.SetColor(0.37f, 0.43f, 0.08f);
            m6.SetSampler(new MultiJittered(vp.NumSamples));

            Plane plane = new Plane(new Vec3(0, 0.01, 0),
                                    new Vec3(0, 1, 0),
                                    m6);
            AddObject(plane);
        }
    }
}
