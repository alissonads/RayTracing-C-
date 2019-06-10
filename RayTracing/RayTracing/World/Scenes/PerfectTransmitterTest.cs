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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.World.Scenes
{
    class PerfectTransmitterTest : Base.SceneBase
    {
        public override string Name
        {
            get { return "PerfectTransmitterTest"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 100;
            vp.MaxDepth = 10;

            backgroundColor = new Vec3(0.0, 0.3, 0.25);
            tracer = new Whitted(this);

            //AmbientOccluder a = new AmbientOccluder(ColorUtils.WHITE,
            //                                        0.25, 1.0);
            //a.Sampler = new MultiJittered(vp.NumSamples);
            Ambient a = new Ambient();
            a.ScaleRadiance = 0.25f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(1, 7.5, 20),
                                          new Vec3(0.0, -0.35, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          5250);

            Camera = pinhole;

            Emissive e = new Emissive();
            e.ScaleRadiance = 3.0f;
            e.Ce = ColorUtils.WHITE;

            RectangularLamp lamp = new RectangularLamp(
                                        new Rectangle(new Vec3(0, 20, 0),
                                                      new Vec3(1, 0, 0),
                                                      new Vec3(0, 0, 1),
                                                      new Vec3(0, -1, 0)),
                                        e);
            lamp.Sampler = new MultiJittered(vp.NumSamples);
            AddObject(lamp);

            AreaLight areaLight = new AreaLight();
            areaLight.Object = lamp;
            areaLight.Shadows = true;
            AddLight(areaLight);

            //PointLight l = new PointLight();
            //l.SetLocation(20, 20, 15);
            //l.ScaleRadiance = 3.0f;
            //AddLight(l);

            Transparent glass = new Transparent();
            glass.SetKs(0.5f);
            glass.SetExp(2000);
            glass.SetIor(1.5f);
            glass.SetKr(0.1f);
            glass.SetKt(1.53f);
            //glass.SetCd(0.4f);

            float ir = 0.9f;
            float or = 1;
            
            //Bowl b = Bowl.CreateFlatRimmedBowl(ir, or);
            Bowl b = Bowl.CreateRoundRimmedBowl(ir, or);
            b.Material = glass;
            AddObject(b);

            Reflective reflective = new Reflective();
            reflective.SetKa(0.6f);
            reflective.SetKd(0.4f);
            reflective.SetCd(ColorUtils.RED);
            reflective.SetKs(0.5f);
            reflective.SetExp(2000);
            reflective.SetKr(0.25f);

            double r = 0.4;
            double t = 55;
            t = MathUtils.PI * t / 180;
            double x = -(0.9 - r) * Math.Cos(t);
            double y = -(0.9 - r) * Math.Sin(t);

            Sphere s = new Sphere(new Vec3(x, y, 0), r, reflective);
            AddObject(s);

            Reflective reflective2 = new Reflective();
            reflective2.SetKa(0.6f);
            reflective2.SetKd(0.4f);
            reflective2.SetCd(ColorUtils.YELLOW);
            reflective2.SetKs(0.5f);
            reflective2.SetExp(2000);
            reflective2.SetKr(0.5f);

            r = 0.35;
            t = 35;
            t = MathUtils.PI * t / 180;
            x =  (0.9 - r) * Math.Cos(t);
            y = -(0.9 - r) * Math.Sin(t);

            Sphere s2 = new Sphere(new Vec3(x, y, 0), r, reflective2);
            AddObject(s2);
            
            Matte rm = new Matte();
            rm.SetColor(ColorUtils.WHITE);
            rm.SetKa(0.8f);
            rm.SetKd(0.85f);

            Rectangle rectangle = new Rectangle(new Vec3(-2, -1, -5),
                                                new Vec3(0, 0, 9),
                                                new Vec3(4, 0, 0));
            rectangle.Material = rm;
            AddObject(rectangle);
        }
    }
}
