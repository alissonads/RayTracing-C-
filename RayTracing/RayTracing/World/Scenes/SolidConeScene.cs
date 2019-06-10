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
using RayTracing.Materials;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Samplers;

namespace RayTracing.World.Scenes
{
    class SolidConeScene : Base.SceneBase
    {
        public override string Name
        {
            get
            {
                return "SolidConeScene";
            }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 4;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            //Ambient a = new Ambient();
            //a.ScaleRadiance = 1.0f;
            //AmbientLight = a;

            AmbientOccluder a = new AmbientOccluder();
            a.ScaleRadiance = 1.5;
            a.Color = ColorUtils.WHITE;
            a.MinAmount = 0.4;
            a.Sampler = new MultiJittered(vp.NumSamples);
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(-150.0, 80.0, 210),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          800);

            Camera = pinhole;

            PointLight l = new PointLight();
            l.Color = ColorUtils.WHITE;
            l.SetLocation(0, 300, 200);
            l.ScaleRadiance = 3.0f;
            l.Shadows = true;
            AddLight(l);

            Phong m = new Phong();
            m.SetColor(ColorUtils.BLUE);
            m.SetKa(0.2f);
            m.SetKd(0.65f);
            m.SetKs(0.4f);
            m.SetExp(64.0f);

            Phong m1 = new Phong();
            m1.SetColor(ColorUtils.YELLOW);
            m1.SetKa(0.2f);
            m1.SetKd(0.65f);
            m1.SetKs(0.4f);
            m1.SetExp(64.0f);

            SolidCone sc = new SolidCone(70, 60);
            sc.SetBodyMaterial(m);
            sc.SetBaseMaterial(m1);

            AddObject(sc);

            Sphere s = new Sphere(new Vec3(150, 0, 0), 50);
            s.Material = m;

            Box b = new Box(-200, -70, -50, 50, -100, 0);
            b.Material = m1;

            AddObject(s);
            AddObject(b);
        }
    }
}
