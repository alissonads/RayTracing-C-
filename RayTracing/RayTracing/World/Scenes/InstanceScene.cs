using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Materials;
using RayTracing.Util;
using RayTracing.Lights;
using RayTracing.Cameras;
using RayTracing.Util.Math;
using RayTracing.Tracers;
using RayTracing.GeometricObjects;
using RayTracing.GeometricObjects.Primitives;

namespace RayTracing.World.Scenes
{
    class InstanceScene : SceneBase
    {
        public override string Name
        {
            get
            {
                return "InstanceScene";
            }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 16;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(100, 0, 100),
                                          new Vec3(0, 1, 0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          8000);

            Camera = pinhole;

            PointLight l = new PointLight();
            l.Color = ColorUtils.WHITE;
            l.SetLocation(50, 50, 1);
            l.ScaleRadiance = 3.0f;
            l.Shadows = true;
            AddLight(l);

            Phong m = new Phong();
            m.SetColor(new Vec3(0.75));
            m.SetKa(0.25f);
            m.SetKd(0.8f);
            m.SetKs(0.15f);
            m.SetExp(50.0f);

            Instance ellipsoid = new Instance(new Sphere());
            ellipsoid.Material = m;
            ellipsoid.Scale(2, 3, 1);
            ellipsoid.RotateX(-45);
            ellipsoid.Translate(0, 1, 0);
            AddObject(ellipsoid);

            //Plane p = new Plane(new Vec3(0, -10, 0), new Vec3(0, 1, 0));
            //Matte pm = new Matte();
            //pm.SetKa(0.25f);
            //pm.SetKd(0.75f);
            //pm.SetColor(new Vec3(0.3));
            //p.Material = pm;

            //AddObject(p);
        }
    }
}
