using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util;
using RayTracing.Materials;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Util.Math;
using RayTracing.Cameras;
using RayTracing.Tracers;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Part;

namespace RayTracing.World.Scenes
{
    class SolidCylinderScene : Base.SceneBase
    {
        public override string Name
        {
            get
            {
                return "SolidCylinderScene";
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
            l.SetLocation(0, 100, 200);
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

            Phong m2 = new Phong();
            m2.SetColor(ColorUtils.GREEN);
            m2.SetKa(0.2f);
            m2.SetKd(0.65f);
            m2.SetKs(0.4f);
            m2.SetExp(64.0f);

            float t = 20;
            float b = -80;
            float r = 50;

            ConvexPartSphere cps = new ConvexPartSphere(new Vec3(), 50);
            cps.Material = m;

            SolidCylinder sc = new SolidCylinder(b, t, r);
            sc.SetTopMaterial(m);
            sc.SetWallMaterial(m1);
            sc.SetBottomMaterial(m2);
            sc.BoundingBox = new BBox(-r, r, b, t, -r, r);

            //Disk top = new Disk(new Vec3(0, t, 0), new Vec3(0, 1, 0), r);
            //Disk bottom = new Disk(new Vec3(0, b, 0), new Vec3(0, -1, 0), r);
            //OpenCylinder wall = new OpenCylinder(b, t, r);
            //wall.BoundingBox = new BBox(-r, r, b, t, -r, r);

            //top.Material = m;
            //bottom.Material = m1;
            //wall.Material = m1;
            AddObject(sc);
            //AddObject(top);
            //AddObject(wall);
        }
    }
}
