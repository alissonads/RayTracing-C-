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
using RayTracing.GeometricObjects;

namespace RayTracing.World.Scenes
{
    class TessellateSphere : SceneBase
    {
        public override string Name
        {
            get { return "TessellateSphere"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 4;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(0.0, 80.0, 210),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          500);

            Camera = pinhole;

            PointLight l = new PointLight();
            l.Color = ColorUtils.WHITE;
            l.SetLocation(100, 100, 200);
            l.ScaleRadiance = 3.0f;
            l.Shadows = true;
            AddLight(l);

            Phong m = new Phong();
            m.SetColor(0.2f, 0.5f, 0.4f);
            m.SetKa(0.25f);
            m.SetKd(0.75f);
            m.SetKs(0.2f);
            m.SetExp(20.0f);

            Grid sphere = new Grid();
            //sphere.TessellateFlatSphere(200, 100);
            sphere.TessellateSmoothSphere(100, 50);
            sphere.Material = m;
            sphere.SetupCells();

            Instance i = new Instance(sphere);
            i.Scale(60.0f);

            AddObject(i);
        }
    }
}
