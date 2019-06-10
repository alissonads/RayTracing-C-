using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Tracers;
using RayTracing.Cameras;
using RayTracing.GeometricObjects.Composite;

namespace RayTracing.World.Scenes
{
    class BeveledCylinderScene : SceneBase
    {
        public override string Name
        {
            get
            {
                return "BeveledCylinderScene";
            }
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
            m.SetColor(new Vec3(0.35f));
            m.SetKa(0.2f);
            m.SetKd(0.65f);
            m.SetKs(0.4f);
            m.SetExp(64.0f);

            float t = 20;
            float b = -80;
            float r = 50;

            //BeveledCylinder bc = new BeveledCylinder(b, t, r, 6);
            //bc.Material = m;
            Cylinder c = Cylinder.Create(b, t, r, 3);
            c.Material = m;

            AddObject(c);
        }
    }
}
