using RayTracing.Cameras;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
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
    class GlossyReflectorTest1 : SceneBase
    {
        public override string Name
        {
            get { return "GlossyReflectorTest1"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 64;
            vp.MaxDepth = 10;

            backgroundColor = ColorUtils.BLACK;
            tracer = new Whitted(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(7.5, 3, 9.5),
                                          new Vec3(5.0, 2.5, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          800);

            Camera = pinhole;

            PointLight l1 = new PointLight();
            l1.Color = ColorUtils.WHITE;
            l1.SetLocation(10, 10, 0);
            l1.ScaleRadiance = 2.0f;
            AddLight(l1);

            PointLight l2 = new PointLight();
            l2.Color = ColorUtils.WHITE;
            l2.SetLocation(0, 10, 10);
            l2.ScaleRadiance = 2.0f;
            AddLight(l2);

            PointLight l3 = new PointLight();
            l3.Color = ColorUtils.WHITE;
            l3.SetLocation(-10, 10, 0);
            l3.ScaleRadiance = 2.0f;
            AddLight(l3);

            PointLight l4 = new PointLight();
            l4.Color = ColorUtils.WHITE;
            l4.SetLocation(0, 10, -10);
            l4.ScaleRadiance = 2.0f;
            AddLight(l4);

            //sphere
            Reflective r1 = new Reflective();
            r1.SetKa(0.1f);
            r1.SetKd(0.4f);
            r1.SetCd(0, 0, 1);
            r1.SetKs(0.25f);
            r1.SetExp(100.0f);
            r1.SetKr(0.85f);
            r1.SetReflectiveColor(0.75f, 0.75f, 1.0f);
            //r1.SetSpecularColor(ColorUtils.WHITE);

            Sphere s1 = new Sphere(new Vec3(0, 0.5, 0), 4);
            s1.Material = r1;
            AddObject(s1);

            //wall
            double roomSize = 11.0;

            //floor
            Matte m1 = new Matte(new Vec3(0.25), 0.1f, 0.50f);

            Plane floor = new Plane(new Vec3(0, -roomSize, 0), new Vec3(0, 1, 0));
            floor.Material = m1;
            AddObject(floor);

            //ceiling
            Matte m2 = new Matte(ColorUtils.WHITE, 0.35f, 0.50f);

            Plane ceiling = new Plane(new Vec3(0, roomSize, 0), new Vec3(0, -1, 0));
            ceiling.Material = m2;
            AddObject(ceiling);

            //back
            Matte m3 = new Matte(new Vec3(0.5, 0.75, 0.75), 0.15f, 0.60f);

            Plane back = new Plane(new Vec3(0, 0, -roomSize), new Vec3(0, 0, 1));
            back.Material = m3;
            AddObject(back);

            //front
            Plane front = new Plane(new Vec3(0, 0, roomSize), new Vec3(0, 0, -1));
            front.Material = m3;
            AddObject(front);

            //left
            Matte m4 = new Matte(new Vec3(0.71, 0.40, 0.20), 0.15f, 0.60f);

            Plane left = new Plane(new Vec3(-roomSize, 0, 0), new Vec3(1, 0, 0));
            left.Material = m4;
            AddObject(left);

            //front
            Plane right = new Plane(new Vec3(roomSize, 0, 0), new Vec3(-1, 0, 0));
            right.Material = m4;
            AddObject(right);

            //mirrors
            double mirrorSize = 8;
            double offset = 1.0;

            Reflective r2 = new Reflective();
            r2.SetKa(0.0f);
            r2.SetKd(0.0f);
            r2.SetCd(0, 0, 0);
            r2.SetKs(0.0f);
            r2.SetExp(2.0f);
            r2.SetKr(0.9f);
            r2.SetReflectiveColor(0.9f, 1.0f, 0.9f);
            //r2.SetSpecularColor(ColorUtils.WHITE);

            float e = 25000;
            GlossyReflector g = new GlossyReflector();
            g.SetSamples(vp.NumSamples, e);
            g.SetKa(0.0f);
            g.SetKd(0.0f);
            g.SetKs(0.0f);
            g.SetExp(e);
            g.SetCd(0, 0, 0);
            g.SetKr(0.9f);
            g.SetExponent(e);
            g.SetReflectiveColor(0.9f, 1.0f, 0.9f);
            //r.SetSpecularColor(ColorUtils.WHITE);
            //back
            Rectangle rect1 = new Rectangle(new Vec3(-mirrorSize, -mirrorSize, -(roomSize - offset)),
                                            new Vec3(2.0 * mirrorSize, 0, 0),
                                            new Vec3(0, 2.0 * mirrorSize, 0),
                                            new Vec3(0, 0, 1),
                                            g);
            AddObject(rect1);

            //front
            Rectangle rect2 = new Rectangle(new Vec3(-mirrorSize, -mirrorSize, +(roomSize - offset)),
                                            new Vec3(2.0 * mirrorSize, 0, 0),
                                            new Vec3(0, 2.0 * mirrorSize, 0),
                                            new Vec3(0, 0, -1),
                                            g);
            AddObject(rect2);

            //left
            Rectangle rect3 = new Rectangle(new Vec3(-(roomSize - offset), -mirrorSize, mirrorSize),
                                            new Vec3(0, 0, -2.0 * mirrorSize),
                                            new Vec3(0, 2.0 * mirrorSize, 0),
                                            new Vec3(1, 0, 0),
                                            r2);
            AddObject(rect3);

            //
            Reflective r3 = new Reflective();
            r3.SetKa(0.0f);
            r3.SetKd(0.0f);
            r3.SetCd(0, 0, 0);
            r3.SetKs(0.0f);
            r3.SetExp(2.0f);
            r3.SetKr(1.0f);
            r3.SetReflectiveColor(1.0f, 1.0f, 0.5f);
            //r3.SetSpecularColor(ColorUtils.WHITE);

            Rectangle rect4 = new Rectangle(new Vec3(-mirrorSize, -4.0, -mirrorSize),
                                            new Vec3(0, 0, 2.0 * mirrorSize),
                                            new Vec3(2.0 * mirrorSize, 0, 0),
                                            new Vec3(0, 1, 0),
                                            r3);
            AddObject(rect4);
        }
    }
}
