using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Materials;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Lamps;
using RayTracing.Samplers;
using RayTracing.Lights;
using RayTracing.Util;
using RayTracing.Tracers;
using RayTracing.Cameras;

namespace RayTracing.World.Scenes
{
    class ObjectsSSFloat : SceneBase
    {
        public override string Name
        {
            get { return "ObjectsSSFloat"; }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1920, 1080, SystemOfCoordinates.SSC_FLOAT);
            vp.NumSamples = 4;

            backgroundColor = ColorUtils.BLACK;
            tracer = new AreaLighting(this);

            MultiJittered sampler = new MultiJittered(vp.NumSamples);

            AmbientOccluder occluder = new AmbientOccluder();
            occluder.Color = ColorUtils.WHITE;
            occluder.ScaleRadiance = 1.5;
            occluder.MinAmount = 0.4f;
            occluder.Sampler = sampler;
            AmbientLight = occluder;

            Pinhole pinhole = new Pinhole(new Vec3(-0.731249, -0.199999, -0.3),
                                          new Vec3(-0.562499, -0.185185, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          0.6f, 1.0f);

            Camera = pinhole;

            CreateIllumination();
            CreateWalls();
            CreateObjects();
        }

        private void CreateWalls()
        {
            //chão
            Phong m = new Phong();
            m.SetColor(new Vec3(0.4f, 0.3f, 0.3f));
            m.SetKa(0.2f);
            m.SetKd(0.7f);
            m.SetKs(0.5f);
            m.SetExp(25.0f);

            Plane p = new Plane(new Vec3(0.0, -0.599999, 0.0), new Vec3(0.0, 1.0, 0.0));
            p.Material = m;
            AddObject(p.Clone());

            //parede fundo
            m.SetColor(new Vec3(0.6f, 0.5f, 0.5f));
            m.SetKa(0.2f);
            m.SetKd(1.0f);
            m.SetKs(0.8f);
            m.SetExp(25.0f);

            p.Point.Set(0.0, 0.0, 2.519791);
            p.Normal.Set(0.0, 0.0, -1.0);
            p.Material = m;
            AddObject(p.Clone());

            //parede esquerda
            m.SetColor(new Vec3(0.5f, 0.5f, 0.6f));
            m.SetKa(0.2f);
            m.SetKd(1.0f);
            m.SetKs(0.8f);
            m.SetExp(25.0f);

            p.Point.Set(-1.349999, 0.0, 0.0);
            p.Normal.Set(1.0, 0.0, 0.0);
            p.Material = m;
            AddObject(p.Clone());

            //parede direita
            m.SetColor(new Vec3(0.5f, 0.5f, 0.6f));
            m.SetKa(0.2f);
            m.SetKd(1.0f);
            m.SetKs(0.8f);
            m.SetExp(25.0f);

            p.Point.Set(1.349999, 0.0, 0.0);
            p.Normal.Set(-1.0, 0.0, 0.0);
            p.Material = m;
            AddObject(p.Clone());

            //parede topo
            m.SetColor(new Vec3(0.7f, 0.7f, 0.7f));
            m.SetKa(0.2f);
            m.SetKd(1.0f);
            m.SetKs(0.8f);
            m.SetExp(25.0f);

            p.Point.Set(0.0, 1.518518, 0.0);
            p.Normal.Set(0.0, -1.0, 0.0);
            p.Material = m;
            AddObject(p);
        }

        private void CreateObjects()
        {
            //esfera de cima da caixa
            Phong m = new Phong();
            m.SetColor(new Vec3(0.6, 0.6, 0.6));
            m.SetKa(0.2f);
            m.SetKd(0.7f);
            m.SetKs(0.8f);
            m.SetExp(20.0f);

            Sphere s = new Sphere(new Vec3(0.168749, 0.099999, 0.449999), 0.4);
            s.Material = m;
            AddObject(s.Clone());

            //esfera grande
            m.SetColor(new Vec3(0.7, 0.7, 1.0));
            m.SetKa(0.2f);
            m.SetKd(0.7f);
            m.SetKs(0.8f);
            m.SetExp(20.0f);

            s.Center.Set(-0.562499, -0.159256, 0.449999);
            s.Radius = 0.5;
            s.Material = m;
            AddObject(s);

            //AddObjects(new Box(RT::Vec3f(0.0f, -3.0f, 2.5f), RT::Vec3f(3.0f, 1.5f, 3.0f),
            //    new Phong(RT::Vec3f(0.7f, 0.7f, 1.0f),
            //        0.2f, 0.7f, 0.2f, 20.0f)));
        }

        private void CreateIllumination()
        {
            //luzes
            //direita fundo
            Rectangle r1 = new Rectangle(new Vec3(0.899999, 1.4, 1.349999),
                                         new Vec3(0.4, 0, 0),
                                         new Vec3(0, 0, 0.4),
                                         new Vec3(0, -1, 0));
            r1.Material = new Emissive(new Vec3(1, 1, 1), 20.0f);

            RectangularLamp lamp1 = new RectangularLamp(r1);
            lamp1.Sampler = new MultiJittered(256);

            //direita frente
            Rectangle r2 = new Rectangle(new Vec3(0.899999, 1.4, -0.224999),
                                         new Vec3(0.4, 0, 0),
                                         new Vec3(0, 0, 0.4),
                                         new Vec3(0, -1, 0));
            r2.Material = new Emissive(new Vec3(1, 1, 1), 20.0f);
            
            RectangularLamp lamp2 = new RectangularLamp(r2);
            lamp2.Sampler = new MultiJittered(256);

            //esquerda fundo
            Rectangle r3 = new Rectangle(new Vec3(-1.349999, 1.4, 1.349999),
                                         new Vec3(0.4, 0, 0),
                                         new Vec3(0, 0, 0.4),
                                         new Vec3(0, -1, 0));
            r3.Material = new Emissive(new Vec3(1, 1, 1), 20.0f);

            RectangularLamp lamp3 = new RectangularLamp(r3);
            lamp3.Sampler = new MultiJittered(256);

            //esquerda frente
            Rectangle r4 = new Rectangle(new Vec3(-1.349999, 1.4, -0.224999),
                                         new Vec3(0.4, 0, 0),
                                         new Vec3(0, 0, 0.4),
                                         new Vec3(0, -1, 0));
            r4.Material = new Emissive(new Vec3(1, 1, 1), 20.0f);
               
            RectangularLamp lamp4 = new RectangularLamp(r4);
            lamp4.Sampler = new MultiJittered(256);

            AddObject(lamp1);
            AddObject(lamp2);
            AddObject(lamp3);
            AddObject(lamp4);

            AreaLight al = new AreaLight();
            al.Object = lamp1;
            AddLight(al.Clone());
            al.Object = lamp2;
            AddLight(al.Clone());
            al.Object = lamp3;
            AddLight(al.Clone());
            al.Object = lamp4;
            AddLight(al.Clone());
        }
    }
}
