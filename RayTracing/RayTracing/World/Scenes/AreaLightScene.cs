using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.Materials;
using RayTracing.Lights;
using RayTracing.Cameras;
using RayTracing.Tracers;
using RayTracing.Samplers;
using RayTracing.GeometricObjects.Lamps;

namespace RayTracing.World.Scenes
{
    class AreaLightingScene : SceneBase
    {
        public AreaLightingScene() :
            base()
        {}

        public override string Name
        {
            get { return "AreaLightingScene"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 64;
            vp.ShowOutOfGamut = 2;

            backgroundColor = ColorUtils.BLACK;
            tracer = new AreaLighting(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(0.0, 0.0, 500.0),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          2000.0f);

            Camera = pinhole;

            Emissive emissive = new Emissive();
            emissive.ScaleRadiance = 40.0f;
            emissive.Ce = ColorUtils.WHITE;

            MultiJittered sampler = new MultiJittered(256);
            sampler.Generate();

            Rectangle rectangle = new Rectangle(new Vec3(100.0, 100.0, 200.0),
                                                new Vec3(216.0, 0.0, 0.0),
                                                new Vec3(0.0, 0.0, 216.0));
            rectangle.Shadows = false;

            RectangularLamp lamp = new RectangularLamp(rectangle, emissive);
            lamp.Sampler = sampler;
            AddObject(lamp);

            AreaLight areaLight = new AreaLight();
            areaLight.Object = lamp;
            areaLight.Shadows = true;
            AddLight(areaLight);

            Vec3 lightGreen = new Vec3(0.65f, 1.0f, 0.30f);
            Vec3 green = new Vec3(0.0f, 0.6f, 0.3f);
            Vec3 darkGreen = new Vec3(0.0f, 0.41f, 0.41f);

            Vec3 yellow = new Vec3(1.0f, 1.0f, 0.0f);
            Vec3 darkYellow = new Vec3(0.61f, 0.61f, 0.0f);

            Vec3 lightPurple = new Vec3(0.65f, 0.3f, 1.0f);
            Vec3 darkPurple = new Vec3(0.5f, 0.0f, 1.0f);

            Vec3 brown = new Vec3(0.71f, 0.40f, 0.16f);
            Vec3 orange = new Vec3(1.0f, 0.75f, 0.0f);

            Matte matte = new Matte();
            matte.SetKa(0.2f);
            matte.SetKd(0.5f);
            matte.SetColor(ColorUtils.WHITE);

            Plane p = new Plane(new Vec3(0, -85, 0), new Vec3(0, 1, 0));
            p.Material = matte;
            AddObject(p);

            Matte m = new Matte();
            //Plastic *m = new Plastic();
            m.SetKa(0.2f);
            m.SetKd(0.65f);
            //m.SetKs(0.1f);
            //m.SetExp(8.0f);
            m.SetColor(ColorUtils.WHITE);

            Sphere s = new Sphere(new Vec3(5, 3, 0), 30);
            s.Material = m;

            AddObject((Sphere)s.Clone());

            s.SetCenter(45, -7, -60);
            s.Radius = 20;
            m.SetColor(brown);
            AddObject((Sphere)s.Clone());

            s.SetCenter(40, 43, -100);
            s.Radius = 17;
            m.SetColor(darkGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-20, 28, -15);
            s.Radius = 20;
            m.SetColor(orange);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-25, -7, -35);
            s.Radius = 27;
            m.SetColor(green);
            AddObject((Sphere)s.Clone());

            s.SetCenter(20, -27, -35);
            s.Radius = 25;
            m.SetColor(lightGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(35, 18, -35);
            s.Radius = 22;
            m.SetColor(green);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-57, -17, -50);
            s.Radius = 15;
            m.SetColor(brown);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-47, 16, -80);
            s.Radius = 23;
            m.SetColor(lightGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-15, -32, -60);
            s.Radius = 22;
            m.SetColor(darkGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-35, -37, -80);
            s.Radius = 22;
            m.SetColor(darkYellow);
            AddObject((Sphere)s.Clone());

            s.SetCenter(10, 43, -80);
            s.Radius = 22;
            m.SetColor(darkYellow);
            AddObject((Sphere)s.Clone());

            s.SetCenter(30, -7, -80);
            s.Radius = 10;
            m.SetColor(darkYellow);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-40, 48, -110);
            s.Radius = 18;
            m.SetColor(darkGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-10, 53, -120);
            s.Radius = 18;
            m.SetColor(brown);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-55, -52, -100);
            s.Radius = 10;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(5, -52, -100);
            s.Radius = 15;
            m.SetColor(brown);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-20, -57, -120);
            s.Radius = 15;
            m.SetColor(darkPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(55, -27, -100);
            s.Radius = 17;
            m.SetColor(darkGreen);
            AddObject((Sphere)s.Clone());

            s.SetCenter(50, -47, -120);
            s.Radius = 15;
            m.SetColor(brown);
            AddObject((Sphere)s.Clone());

            s.SetCenter(70, -42, -150);
            s.Radius = 10;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(5, 73, -130);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(66, 21, -130);
            s.Radius = 13;
            m.SetColor(darkPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(72, -12, -140);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(64, 5, -160);
            s.Radius = 11;
            m.SetColor(green);
            AddObject((Sphere)s.Clone());

            s.SetCenter(55, 38, -160);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-73, -2, -160);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(30, -62, -140);
            s.Radius = 15;
            m.SetColor(darkPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(25, 63, -140);
            s.Radius = 15;
            m.SetColor(darkPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-60, 46, -140);
            s.Radius = 15;
            m.SetColor(darkPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-30, 68, -130);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(58, 56, -180);
            s.Radius = 11;
            m.SetColor(green);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-63, -39, -180);
            s.Radius = 11;
            m.SetColor(green);
            AddObject((Sphere)s.Clone());

            s.SetCenter(46, 68, -200);
            s.Radius = 10;
            m.SetColor(lightPurple);
            AddObject((Sphere)s.Clone());

            s.SetCenter(-3, -72, -130);
            s.Radius = 12;
            m.SetColor(lightPurple);
            AddObject(s);
        }
    }
}
