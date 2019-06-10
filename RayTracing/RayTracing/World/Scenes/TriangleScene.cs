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
using RayTracing.GeometricObjects.Triangles;
using RayTracing.Materials;

namespace RayTracing.World.Scenes
{
    class TriangleScene : SceneBase
    {
        public override string Name
        {
            get
            {
                return "TriangleScene";
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

            Pinhole pinhole = new Pinhole(new Vec3(0.0, 0.0, 500.0),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          2000.0f);

            Camera = pinhole;

            PointLight l = new PointLight();
            l.Color = ColorUtils.WHITE;
            l.SetLocation(0, 100, -20);
            l.ScaleRadiance = 20.0f;
            l.Shadows = false;
            AddLight(l);

            Triangle t = new Triangle(new Vec3(0, 50, 0),
                                      new Vec3(-50, -50, 0), 
                                      new Vec3(50, -50, 0));
            Matte matte = new Matte();
            matte.SetKa(0.2f);
            matte.SetKd(0.5f);
            matte.SetColor(new Vec3(0.8, 0.6, 0.2));

            t.Material = matte;
            t.Shadows = false;

            AddObject(t);
        }
    }
}
