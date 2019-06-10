using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.Cameras.Base;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracing.Cameras
{
    class Pinhole : Camera
    {
        private float d;
        private float zoom;

        public float ViewPlaneDistance
        {
            get { return d; }
            set
            {
                d = value;
            }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
            }
        }

        public Pinhole() :
            base()
        {
            d = 500.0f;
            zoom = 1.0f;
        }

        public Pinhole(Vec3 eye, Vec3 lookat, Vec3 up) :
            this(eye, lookat, up, 500.0f)
        {}

        public Pinhole(Vec3 eye, Vec3 lookat, Vec3 up, float vpDistance) :
            this(eye, lookat, up, vpDistance, 1.0f)
        {}

        public Pinhole(Vec3 eye, Vec3 lookat, Vec3 up, 
                       float vpDistance, float zoom) :
            base(eye, lookat, up)
        {
            d = vpDistance;
            this.zoom = zoom;
        }

        public Pinhole(Pinhole other) :
            base(other)
        {
            d = other.d;
            zoom = other.zoom;
        }

        public override Base.Camera Clone()
        {
            return new Pinhole(this);
        }

        public override void RenderScene(SceneBase world)
        {
            ComputeUVW();

            Vec3 pixelColor = new Vec3();
            Ray ray = new Ray();
            Vec2 pp = new Vec2();
            Vec2 sp;
            int depth = 0;
            
            Vec2 s = world.ViewPlane.PixelSize / zoom;

            ray.O = eye;

            int hres = world.ViewPlane.Hres;
            int vres = world.ViewPlane.Vres;
            int numSamples = world.ViewPlane.NumSamples;

            for (int r = 0; r < vres; r++)
                for(int c = 0; c < hres; c++)
                {
                    pixelColor = ColorUtils.BLACK;

                    for (int j = 0; j < numSamples; j++)
                    {
                        sp = world.ViewPlane.Sampler.SampleUnitSquare();
                        pp.X = s.X * (c - 0.5 * hres + sp.X);
                        pp.Y = s.Y * (r - 0.5 * vres + sp.Y);
                        ray.D = RayDirection(pp);
                        pixelColor += world.Tracer.TraceRay(ray, depth);
                    }

                    pixelColor /=  numSamples;
                    pixelColor *=  exposureTime;

                    world.DisplayPixel(c, r, pixelColor);
                }
        }

        public override void MultiThreadRenderScene(SceneBase world)
        {
            ComputeUVW();
            
            var task = new Task[4];

            int r = world.ViewPlane.Vres / 2;
            int c = world.ViewPlane.Hres / 2;
            int vres = world.ViewPlane.Vres;
            int hres = world.ViewPlane.Hres;

            task[0] = Task.Run(() => Process(world, 0, 0, c, r));
            task[1] = Task.Run(() => Process(world, c, 0, hres, r));
            task[2] = Task.Run(() => Process(world, 0, r, c, vres));
            task[3] = Task.Run(() => Process(world, c, r, hres, vres));

            Task.WaitAll(task);
        }
        
        public void Process(SceneBase world, int ci, int ri, int hres, int vres)
        {
            Vec2 s = world.ViewPlane.PixelSize / zoom;
            
            int numSamples = world.ViewPlane.NumSamples;
            
            Vec3 pixelColor = ColorUtils.BLACK;
            Ray ray = new Ray(eye, null);
            Vec2 pp = new Vec2();
            Vec2 sp;

            for (int r = ri; r < vres; r++)     //up - y height
                for (int c = ci; c < hres; c++) //across - x width
                {
                    pixelColor = ColorUtils.BLACK;

                    for (int j = 0; j < numSamples; j++)
                    {
                        sp = world.ViewPlane.Sampler.SampleUnitSquare();
                        pp.X = s.X * (c - 0.5 * world.ViewPlane.Hres + sp.X);
                        pp.Y = s.Y * (r - 0.5 * world.ViewPlane.Vres + sp.Y);
                        ray.D = RayDirection(pp);
                        pixelColor += world.Tracer.TraceRay(ray, 0);
                    }

                    pixelColor /= numSamples;
                    pixelColor *= exposureTime;

                    world.DisplayPixel(c, r, pixelColor);
                }
        }

        private Vec3 RayDirection(Vec2 pp)
        {
            Vec3 dir = pp.X * u + pp.Y * v - d * w;
            dir.Normalize();
            return dir;
        }

    }
}
