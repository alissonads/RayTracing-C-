using RayTracing.Cameras;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Lamps;
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
    class ReflectiveObjects : SceneBase
    {
        public override string Name
        {
            get { return "ReflectiveObjects"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 144;
            vp.MaxDepth = 10;

            backgroundColor = new Vec3(0.15);
            tracer = new AreaLighting(this);

            AmbientOccluder a = new AmbientOccluder();
            a.ScaleRadiance = 0.5;
            a.MinAmount = 0.05;
            a.Sampler = new MultiJittered(vp.NumSamples);
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(75, 40, 100),
                                          new Vec3(-10, 39, 0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          360, 3.0f);

            Camera = pinhole;

            Rectangle rectangle = new Rectangle(new Vec3(75, 40, 100),
                                                new Vec3(0, 50, 0), 
                                                new Vec3(50, 0, -50));
            Emissive e = new Emissive(ColorUtils.WHITE, 20);
            RectangularLamp lamp = new RectangularLamp(rectangle, e);
            lamp.Sampler = new MultiJittered(vp.NumSamples);
            lamp.Shadows = false;

            AddObject(lamp);

            AreaLight light = new AreaLight();
            light.Object = lamp;

            AddLight(light);

            Reflective reflective1 = new Reflective();
            reflective1.SetKa(0.25f);
            reflective1.SetKd(0.5f);
            reflective1.SetColor(0.75f, 0.75f, 0.0f);
            reflective1.SetSpecularColor(ColorUtils.WHITE);
            reflective1.SetKs(0.15f);
            reflective1.SetExp(100.0f);
            reflective1.SetKr(0.75f);
            reflective1.SetReflectiveColor(ColorUtils.WHITE);

            double radius = 23.0;
            Sphere s1 = new Sphere(new Vec3(38, radius, -25), radius);
            s1.Material = reflective1;
            AddObject(s1);

            Matte matte1 = new Matte();
            matte1.SetKa(0.45f);
            matte1.SetKd(0.75f);
            matte1.SetColor(0.75f, 0.25f, 0.0f);

            Sphere s2 = new Sphere(new Vec3(-7, 10, 42), 20);
            s2.Material = matte1;
            AddObject(s2);

            Reflective reflective2 = new Reflective();
            reflective2.SetKa(0.35f);
            reflective2.SetKd(0.75f);
            reflective2.SetColor(ColorUtils.BLACK);
            reflective2.SetSpecularColor(ColorUtils.WHITE);
            reflective2.SetKs(0.0f);
            reflective2.SetExp(1.0f);
            reflective2.SetKr(0.75f);
            reflective2.SetReflectiveColor(ColorUtils.WHITE);

            Sphere s3 = new Sphere(new Vec3(-30, 59, 35), 20);
            s3.Material = reflective2;
            AddObject(s3);

            //cylinder
            Reflective reflective3 = new Reflective();
            reflective3.SetKa(0.35f);
            reflective3.SetKd(0.5f);
            reflective3.SetColor(0.0f, 0.5f, 0.75f);
            reflective3.SetSpecularColor(ColorUtils.WHITE);
            reflective3.SetKs(0.2f);
            reflective3.SetExp(100.0f);
            reflective3.SetKr(0.75f);
            reflective3.SetReflectiveColor(ColorUtils.WHITE);

            float b = 0.0f;
            float t = 85f;
            float cr = 22f;

            SolidCylinder cylinder = new SolidCylinder(b, t, cr);
            cylinder.Material = reflective3;
            AddObject(cylinder);

            //box
            Matte matte2 = new Matte();
            matte2.SetKa(0.15f);
            matte2.SetKd(0.5f);
            matte2.SetColor(0.75f, 1.0f, 0.75f);

            Box box = new Box(new Vec3(-35, 0, -110), 
                              new Vec3(-25, 60, 65));
            box.Material = matte2;
            AddObject(box);

            //plane
            Matte matte3 = new Matte();
            matte3.SetKa(0.30f);
            matte3.SetKd(0.9f);
            matte3.SetColor(ColorUtils.WHITE);

            Plane plane = new Plane(new Vec3(), 
                                    new Vec3(0, 1, 0));
            plane.Material = matte3;

            AddObject(plane);
        }
    }
}
