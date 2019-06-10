using RayTracing.Cameras;
using RayTracing.GeometricObjects;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Lights;
using RayTracing.Materials;
using RayTracing.Textures.Images;
using RayTracing.Textures.Mappings;
using RayTracing.Tracers;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Util.PPMFile;
using RayTracing.World.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracing.World.Scenes
{
    class Earth : SceneBase
    {
        const string PATH = "../../Images/";

        public override string Name
        {
            get
            {
                return "Earth";
            }
        }

        public override void Build()
        {
            vp = ViewPlane.Create(1024, 768, SystemOfCoordinates.SSC_INT);
            vp.NumSamples = 25;

            backgroundColor = ColorUtils.BLACK;
            tracer = new RayCast(this);

            Ambient a = new Ambient();
            a.ScaleRadiance = 1.0f;
            AmbientLight = a;

            Pinhole pinhole = new Pinhole(new Vec3(0, 0, 65),
                                          new Vec3(0.0, 0.0, 0.0),
                                          new Vec3(0.0, 1.0, 0.0),
                                          9000/**24*/);

            Camera = pinhole;

            Directional light = new Directional();
            light.SetDirection(-0.25f, 0.4f, 1.0f);
            light.ScaleRadiance = 2.5f;
            AddLight(light);
            
            //image
            Image image = new Image();
            image.Load(PATH + "ppm/EarthLowRes.ppm");
            
            //mapping
            SphericalMap sphericalMap = new SphericalMap();

            //image based texture
            ImageTexture texture = new ImageTexture(image, sphericalMap);

            //textured material
            SV_Matte svMatte = new SV_Matte();
            svMatte.SetKa(0.45f);
            svMatte.SetKd(0.65f);
            svMatte.SetCd(texture);

            Sphere s = new Sphere();
            s.Material = svMatte;

            Instance earth = new Instance(s);
            earth.Material = svMatte;
            earth.RotateY(-72);
            earth.RotateX(40);
            earth.RotateZ(20);
            AddObject(earth);
        }
    }
}
