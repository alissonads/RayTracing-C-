using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util.Math;
using RayTracing.Cameras;
using RayTracing.Lights;
using RayTracing.Util;
using RayTracing.Tracers;
using RayTracing.Materials;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Samplers;
using RayTracing.GeometricObjects.Lamps;

namespace RayTracing.World.Scenes
{
    class BowlScene : SceneBase
    {
        public override string Name
        {
            get
            {
                return "BowlScene";
            }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 64;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            AmbientOccluder a = new AmbientOccluder(ColorUtils.WHITE,
                                                    0.25, 1.0);
            a.Sampler = new MultiJittered(vp.NumSamples);
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(0.0, 80.0, 210),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          500);

            Camera = pinhole;

            Emissive e = new Emissive();
            e.ScaleRadiance = 8.0f;
            e.Ce = ColorUtils.WHITE;

            Rectangle rectangle = new Rectangle(new Vec3(0.0, 200.0, 100.0),
                                                new Vec3(216.0, 0.0, 0.0),
                                                new Vec3(0.0, 0.0, 216.0));
            rectangle.Shadows = false;

            RectangularLamp lamp = new RectangularLamp(rectangle, e);
            lamp.Sampler = new MultiJittered(vp.NumSamples);
            AddObject(lamp);

            AreaLight areaLight = new AreaLight();
            areaLight.Object = lamp;
            areaLight.Shadows = true;
            AddLight(areaLight);

            //PointLight l = new PointLight();
            //l.Color = ColorUtils.WHITE;
            //l.SetLocation(100, 100, 200);
            //l.ScaleRadiance = 3.0f;
            //l.Shadows = true;
            //AddLight(l);

            Phong m = new Phong();
            m.SetColor(new Vec3(0.6, 0.2, 0.02));
            m.SetKa(0.2f);
            m.SetKd(0.65f);
            m.SetKs(0.4f);
            m.SetExp(64.0f);

            Phong i = new Phong();
            i.SetColor(new Vec3(0.8, 0.4, 0.02));
            i.SetKa(0.2f);
            i.SetKd(0.65f);
            i.SetKs(0.4f);
            i.SetExp(64.0f);

            //Bowl b = Bowl.CreateFlatRimmedBowl(75, 80);
            //Bowl b = Bowl.CreateRoundRimmedBowl(75, 80);
            Bowl b = Bowl.Create(75, 80, true);
                
            b.SetExternalMaterial(m);
            b.SetInternalMaterial(i);
            b.SetBorderMaterial(i);

            AddObject(b);

            Plane p = new Plane(new Vec3(0, -80, 0), new Vec3(0, 1, 0));
            Matte pm = new Matte();
            pm.SetColor(new Vec3(0.2, 0.2, 0.2));
            pm.SetKa(0.2f);
            pm.SetKd(0.6f);
            p.Material = pm;

            AddObject(p);
        }
    }
}
