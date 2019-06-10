using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util;
using RayTracing.Tracers;
using RayTracing.Samplers;
using RayTracing.Lights;
using RayTracing.Cameras;
using RayTracing.Util.Math;
using RayTracing.Materials;
using RayTracing.GeometricObjects.Primitives;

namespace RayTracing.World.Scenes
{
    class AmbientOcclusion : SceneBase
    {
        public AmbientOcclusion() :
            base()
        {}

        public override string Name
        {
            get { return "AmbientOcclusion"; }
        }

        public override void Build()
        {
            int numSamples = 4;

            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = numSamples;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            MultiJittered sampler = new MultiJittered(numSamples);
            sampler.Generate();

            AmbientOccluder occluder = new AmbientOccluder();
            occluder.ScaleRadiance = 1.0;
            occluder.Color = ColorUtils.WHITE;
            occluder.MinAmount = 0.0;
            occluder.Sampler = sampler;
            AmbientLight = occluder;

            Pinhole pinhole = new Pinhole(new Vec3(25.0, 20.0, 45.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          5000.0f);
            pinhole.ComputeUVW();
            Camera = pinhole;

            Matte matte = new Matte();
            matte.SetKa(0.75f);
            matte.SetKd(0.0f);
            matte.SetColor(1.0f, 0.7f, 0.0f);

            Sphere s = new Sphere(new Vec3(0, 1, 0), 1);
            s.Material = matte;
            AddObject(s);

            matte = new Matte();
            matte.SetKa(0.75f);
            matte.SetKd(0.0f);
            matte.SetColor(ColorUtils.WHITE);

            Plane p = new Plane(new Vec3(0, 0, 0), new Vec3(0, 1, 0));
            p.Material = matte;
            AddObject(p);
        }
    }
}
