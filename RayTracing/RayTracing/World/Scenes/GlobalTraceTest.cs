using RayTracing.Cameras;
using RayTracing.GeometricObjects.Lamps;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Samplers;
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
    class GlobalTraceTest : SceneBase
    {
        public override string Name
        {
            get { return "GlobalTraceTest"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            //vp.NumSamples = 4096;
            vp.NumSamples = 400;
            vp.MaxDepth = 10;

            backgroundColor = ColorUtils.BLACK;

            tracer = new GlobalTrace(this);
            //tracer = new PathTrace(this);

            Ambient ambient = new Ambient();
            ambient.ScaleRadiance = 0;
            AmbientLight = ambient;

            Pinhole camera = new Pinhole();
            camera.SetEye(27.6, 27.4, -80);
            camera.SetLookat(27.6, 27.4, 0.0);
            camera.ViewPlaneDistance = 1200;
            Camera = camera;

            Vec3 p0;
            Vec3 a, b;
            Vec3 normal;

            // box dimensions

            double width = 55.28;       // x direction
            double height = 54.88;      // y direction
            double depth = 55.92;      // z direction


            Emissive emissive = new Emissive(new Vec3(1.0, 0.73, 0.4), 100);
            
            p0 = new Vec3(21.3, height - 0.001, 22.7);
            a = new Vec3(0.0, 0.0, 10.5);
            b = new Vec3(13.0, 0.0, 0.0);
            normal = new Vec3(0.0, -1.0, 0.0);

            RectangularLamp lamp = new RectangularLamp(new Rectangle(p0, a, b, normal), emissive);
            lamp.Sampler = new MultiJittered(vp.NumSamples);
            AddObject(lamp);

            AreaLight ceilingLight = new AreaLight();
            ceilingLight.Object = lamp;
            AddLight(ceilingLight);


            // left wall

            Matte matte1 = new Matte();
            matte1.SetKa(0.0f);
            matte1.SetKd(0.6f);
            matte1.SetColor(0.57f, 0.025f, 0.025f);     // red
            matte1.SetSampler(new MultiJittered(vp.NumSamples));

            p0 = new Vec3(width, 0.0, 0.0);
            a = new Vec3(0.0, 0.0, depth);
            b = new Vec3(0.0, height, 0.0);
            normal = new Vec3(-1.0, 0.0, 0.0);
            Rectangle leftWall = new Rectangle(p0, a, b, normal);
            leftWall.Material = matte1;
            AddObject(leftWall);


            // right wall

            Matte matte2 = new Matte();
            matte2.SetKa(0.0f);
            matte2.SetKd(0.6f);
            matte2.SetColor(0.37f, 0.59f, 0.2f);    // green  
            matte2.SetSampler(new MultiJittered(vp.NumSamples));

            p0 = new Vec3(0.0, 0.0, 0.0);
            a = new Vec3(0.0, 0.0, depth);
            b = new Vec3(0.0, height, 0.0);
            normal = new Vec3(1.0, 0.0, 0.0);
            Rectangle rightWall = new Rectangle(p0, a, b, normal);
            rightWall.Material = matte2;
            AddObject(rightWall);


            // back wall

            Matte matte3 = new Matte();
            matte3.SetKa(0.0f);
            matte3.SetKd(0.6f);
            matte3.SetColor(ColorUtils.WHITE);
            matte3.SetSampler(new MultiJittered(vp.NumSamples));

            p0 = new Vec3(0.0, 0.0, depth);
            a = new Vec3(width, 0.0, 0.0);
            b = new Vec3(0.0, height, 0.0);
            normal = new Vec3(0.0, 0.0, -1.0);
            Rectangle backWall = new Rectangle(p0, a, b, normal);
            backWall.Material = matte3;
            AddObject(backWall);


            // floor

            p0 = new Vec3(0.0, 0.0, 0.0);
            a = new Vec3(0.0, 0.0, depth);
            b = new Vec3(width, 0.0, 0.0);
            normal = new Vec3(0.0, 1.0, 0.0);
            Rectangle floor = new Rectangle(p0, a, b, normal);
            floor.Material = matte3;
            AddObject(floor);


            // ceiling

            p0 = new Vec3(0.0, height, 0.0);
            a = new Vec3(0.0, 0.0, depth);
            b = new Vec3(width, 0.0, 0.0);
            normal = new Vec3(0.0, -1.0, 0.0);
            Rectangle ceiling = new Rectangle(p0, a, b, normal);
            ceiling.Material = matte3;
            AddObject(ceiling);


            // the two boxes defined as 5 rectangles each

            // short box

            // top

            p0 = new Vec3(13.0, 16.5, 6.5);
            a = new Vec3(-4.8, 0.0, 16.0);
            b = new Vec3(16.0, 0.0, 4.9);
            normal = new Vec3(0.0, 1.0, 0.0);
            Rectangle shortTop = new Rectangle(p0, a, b, normal);
            shortTop.Material = matte3;
            AddObject(shortTop);


            // side 1

            p0 = new Vec3(13.0, 0.0, 6.5);
            a = new Vec3(-4.8, 0.0, 16.0);
            b = new Vec3(0.0, 16.5, 0.0);
            Rectangle shortSide1 = new Rectangle(p0, a, b);
            shortSide1.Material = matte3;
            AddObject(shortSide1);


            // side 2

            p0 = new Vec3(8.2, 0.0, 22.5);
            a = new Vec3(15.8, 0.0, 4.7);
            Rectangle shortSide2 = new Rectangle(p0, a, b);
            shortSide2.Material = matte3;
            AddObject(shortSide2);


            // side 3

            p0 = new Vec3(24.2, 0.0, 27.4);
            a = new Vec3(4.8, 0.0, -16.0);
            Rectangle shortSide3 = new Rectangle(p0, a, b);
            shortSide3.Material = matte3;
            AddObject(shortSide3);


            // side 4

            p0 = new Vec3(29.0, 0.0, 11.4);
            a = new Vec3(-16.0, 0.0, -4.9);
            Rectangle shortSide4 = new Rectangle(p0, a, b);
            shortSide4.Material = matte3;
            AddObject(shortSide4);



            // tall box

            // top

            p0 = new Vec3(42.3, 33.0, 24.7);
            a = new Vec3(-15.8, 0.0, 4.9);
            b = new Vec3(4.9, 0.0, 15.9);
            normal = new Vec3(0.0, 1.0, 0.0);
            Rectangle tallTop = new Rectangle(p0, a, b, normal);
            tallTop.Material = matte3;
            AddObject(tallTop);


            // side 1

            p0 = new Vec3(42.3, 0.0, 24.7);
            a = new Vec3(-15.8, 0.0, 4.9);
            b = new Vec3(0.0, 33.0, 0.0);
            Rectangle tallSide1 = new Rectangle(p0, a, b);
            tallSide1.Material = matte3;
            AddObject(tallSide1);


            // side 2

            p0 = new Vec3(26.5, 0.0, 29.6);
            a = new Vec3(4.9, 0.0, 15.9);
            Rectangle tallSide2 = new Rectangle(p0, a, b);
            tallSide2.Material = matte3;
            AddObject(tallSide2);


            // side 3

            p0 = new Vec3(31.4, 0.0, 45.5);
            a = new Vec3(15.8, 0.0, -4.9);
            Rectangle tallSide3 = new Rectangle(p0, a, b);
            tallSide3.Material = matte3;
            AddObject(tallSide3);


            // side 4

            p0 = new Vec3(47.2, 0.0, 40.6);
            a = new Vec3(-4.9, 0.0, -15.9);
            Rectangle tallSide4 = new Rectangle(p0, a, b);
            tallSide4.Material = matte3;
            AddObject(tallSide4);
        }
    }
}
