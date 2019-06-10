using RayTracing.Cameras;
using RayTracing.GeometricObjects;
using RayTracing.GeometricObjects.Composite;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Textures.Procedural;
using RayTracing.Tracers;
using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.World.Scenes
{
    class GlassWithLiquidScene : Base.SceneBase
    {
        public override string Name
        {
            get
            {
                return "GlassWithLiquid";
            }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 16;
            vp.MaxDepth = 5;

            backgroundColor = new Vec3(0.5);
            tracer = new Whitted(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 0.5f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(5, 6, 10),
                                          new Vec3(0.0, 1.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          2000);

            Camera = pinhole;

            PointLight light1 = new PointLight();
            light1.SetLocation(40, 50, 30);
            light1.ScaleRadiance = 3.0f;
            AddLight(light1);

            Vec3 glassColor = new Vec3(0.65, 1.0, 0.75);
            Vec3 liquidColor = new Vec3(1.0, 0.25, 1.0);

            Dielectric glass = new Dielectric();
            glass.SetEtaIn(1.50f);
            glass.SetEtaOut(1.0f);
            glass.SetCfIn(glassColor);
            glass.SetCfOut(1, 1, 1);

            Dielectric liquid = new Dielectric();
            liquid.SetEtaIn(1.33f);
            liquid.SetEtaOut(1.0f);
            liquid.SetCfIn(liquidColor);
            liquid.SetCfOut(1, 1, 1);

            Dielectric dielectric = new Dielectric();
            dielectric.SetEtaIn(1.33f);
            dielectric.SetEtaOut(1.50f);
            dielectric.SetCfIn(liquidColor);
            dielectric.SetCfOut(glassColor);

            //double height = 2.0;
            //double innerRadius = 0.9;
            //double wallThickness = 0.1;
            //double baseThickness = 0.3;
            //double waterHeight = 1.5;
            //double meniscusRadius = 0.1;

            double glassHeight = 2.0;
            double liquidHeight = 1.5;
            double innerRadius = 0.991;
            double outerRadius = 1.0;

            GlassWithLiquid glassWithLiquid = new GlassWithLiquid(glassHeight,
                                                                  liquidHeight,
                                                                  innerRadius,
                                                                  outerRadius);

            //GlassWithLiquid glassWithLiquid = new GlassWithLiquid(
            //                                                      height,
            //                                                      innerRadius,
            //                                                      wallThickness,
            //                                                      baseThickness,
            //                                                      waterHeight,
            //                                                      meniscusRadius);

            glassWithLiquid.SetGlassAirMaterial(glass);
            glassWithLiquid.SetLiquidAirMaterial(liquid);
            glassWithLiquid.SetLiquidGlassMaterial(dielectric);
            AddObject(glassWithLiquid);

            Matte matte = new Matte();
            matte.SetColor(1, 1, 0);
            matte.SetKa(0.25f);
            matte.SetKd(0.65f);

            Instance straw = new Instance(new OpenCylinder(-1.2, 1.7, 0.05));
            straw.Material = matte;
            straw.RotateZ(40);
            straw.Translate(0, 1.25f, 0);
            //straw.ComputeBoundingBox();
            AddObject(straw);

            // ground plane
            Checker3D checker = new Checker3D();
            checker.Size = 0.5f;
            checker.SetColor1(0.75f);
            checker.SetColor2(1);
            
            SV_Matte svMatte = new SV_Matte();
            svMatte.SetKa(0.5f);
            svMatte.SetKd(0.75f);
            svMatte.SetCd(checker);

            Plane plane = new Plane(new Vec3(0, -0.01, 0), 
                                    new Vec3(0, 1, 0));
            plane.Material = svMatte;
            AddObject(plane);
        }
    }
}
