using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Cameras;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.Tracers;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Samplers;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.GeometricObjects.Lamps;
using RayTracing.World.Base;
using RayTracing.GeometricObjects;

namespace RayTracing.World.Scenes
{
    class ObjectTest : SceneBase
    {
        public ObjectTest() :
            base()
        {}
        
        public override string Name
        {
            get { return "ObjectTest"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 144;

            backgroundColor = ColorUtils.BLACK;
            //tracer = new RayCast(this);
            tracer = new AreaLighting(this);

            AmbientOccluder a = new AmbientOccluder(ColorUtils.WHITE,
                                                    0.4, 1.5);
            a.Sampler = new MultiJittered(vp.NumSamples);
            AmbientLight = a;

            Pinhole c = new Pinhole(new Vec3(-702, -108, 270),
                                    new Vec3(-540, -100, 0),
                                    new Vec3(0, 1, 0),
                                    800, 1.5f);

            Camera = c;



            CreateIllumination();
            CreateWalls();
            CreateObjects();
            
        }

        private void CreateWalls()
        {
            //chão
            Matte m1 = new Matte();
            m1.SetColor(new Vec3(0.4, 0.3, 0.3));
            m1.SetKa(0.2f);
            m1.SetKd(0.7f);
            Plane p1 = new Plane(new Vec3(0, -324, 0), new Vec3(0, 1, 0));
            p1.Material = m1;

            //fundos
            Matte m2 = new Matte();
            m2.SetColor(new Vec3(0.6, 0.5, 0.5));
            m2.SetKa(0.2f);
            m2.SetKd(1.0f);
            Plane p2 = new Plane(new Vec3(0, 0, -2419), new Vec3(0, 0, 1));
            p2.Material = m2;

            //esquerda
            Matte m3 = new Matte();
            m3.SetColor(new Vec3(0.5, 0.5, 0.6));
            m3.SetKa(0.2f);
            m3.SetKd(1.0f);
            Plane p3 = new Plane(new Vec3(-1296, 0, 0), new Vec3(1, 0, 0));
            p3.Material = m3;

            //direita
            Matte m4 = new Matte();
            m4.SetColor(new Vec3(0.5, 0.5, 0.6));
            m4.SetKa(0.2f);
            m4.SetKd(1.0f);
            Plane p4 = new Plane(new Vec3(1296, 0, 0), new Vec3(-1, 0, 0));
            p4.Material = m4;

            //teto
            Matte m5 = new Matte();
            m5.SetColor(new Vec3(0.7, 0.7, 0.7));
            m5.SetKa(0.2f);
            m5.SetKd(0.7f);
            Plane p5 = new Plane(new Vec3(0, 820, 0), new Vec3(0, -1, 0));
            p5.Material = m5;

            AddObject(p1);
            AddObject(p2);
            AddObject(p3);
            AddObject(p4);
            AddObject(p5);
        }

        private void CreateObjects()
        {
            Phong m1 = new Phong();
            m1.SetColor(new Vec3(0.6, 0.6, 0.6));
            m1.SetKa(0.2f);
            m1.SetKd(0.7f);
            m1.SetKs(0.8f);
            m1.SetExp(20.0f);

            Sphere s1 = new Sphere(new Vec3(/*300, 54, -432*/), 216);
            s1.Material = m1;

            Instance i = new Instance(s1);
            i.Scale(1.0f, 2.0f, 0.9f);
            i.RotateX(90);
            i.Translate(300, 54, -432);
            AddObject(i);

            Phong m2 = new Phong();
            m2.SetColor(new Vec3(0.7, 0.7, 1.0));
            m2.SetKa(0.2f);
            m2.SetKd(0.7f);
            m2.SetKs(0.8f);
            m2.SetExp(20.0f);

            Sphere s2 = new Sphere(new Vec3(/*-540, -86, -432*/), 270);
            s2.Material = m2;

            Instance i2 = new Instance(s2);
            i2.Translate(-540, -86, -432);
            AddObject(i2);

            Phong m3 = new Phong();
            m3.SetColor(new Vec3(0.7, 0.7, 1.0));
            m3.SetKa(0.2f);
            m3.SetKd(0.7f);
            m3.SetKs(0.2f);
            m3.SetExp(1.0f);

            Box b = new Box(new Vec3(138, -324, -594),
                            new Vec3(462, -162, -270));
            b.Material = m3;
            AddObject(b);
        }

        private void CreateIllumination()
        {
            //PointLight l = new PointLight();
            //l.Color = ColorUtils.WHITE;
            //l.SetLocation(0, 300, 200);
            //l.ScaleRadiance = 3.0f;
            //l.Shadows = true;
            //AddLight(l);

            Spherical();
        }

        private void Spherical()
        {
            Emissive e = new Emissive(ColorUtils.WHITE, 28);
            
            Sphere s1 = new Sphere(new Vec3(972, 756, 108), 120);
            SphericalLamp sl1 = new SphericalLamp(s1, e);
            sl1.Shadows = false;
            sl1.Sampler = new MultiJittered(vp.NumSamples);

            Sphere s2 = new Sphere(new Vec3(972, 756, -1296), 120);
            SphericalLamp sl2 = new SphericalLamp(s2, e);
            sl2.Shadows = false;
            sl2.Sampler = new MultiJittered(vp.NumSamples);

            Sphere s3 = new Sphere(new Vec3(-1188, 756, 108), 120);
            SphericalLamp sl3 = new SphericalLamp(s3, e);
            sl3.Shadows = false;
            sl3.Sampler = new MultiJittered(vp.NumSamples);

            Sphere s4 = new Sphere(new Vec3(-1188, 756, -1296), 120);
            SphericalLamp sl4 = new SphericalLamp(s4, e);
            sl4.Shadows = false;
            sl4.Sampler = new MultiJittered(vp.NumSamples);

            AddObject(sl1);
            AddObject(sl2);
            AddObject(sl3);
            AddObject(sl4);

            AreaLight l1 = new AreaLight();
            l1.Object = sl1;
            l1.Shadows = true;

            AreaLight l2 = new AreaLight();
            l2.Object = sl2;
            l2.Shadows = true;

            AreaLight l3 = new AreaLight();
            l3.Object = sl3;
            l3.Shadows = true;

            AreaLight l4 = new AreaLight();
            l4.Object = sl4;
            l4.Shadows = true;

            AddLight(l1);
            AddLight(l2);
            AddLight(l3);
            AddLight(l4);
        }
    }

}

