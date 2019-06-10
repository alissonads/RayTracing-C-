using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RayTracing.Util.Math;
using RayTracing.Tracers.Base;
using RayTracing.Cameras.Base;
using RayTracing.Util;
using RayTracing.Lights.Base;

namespace RayTracing.World.Base
{
    using GeometricObjects.Base;
    using Observer;

    abstract class SceneBase : IScene, ISceneObservable
    {
        protected ViewPlane vp;
        protected Vec3 backgroundColor;
        protected Tracer tracer;
        private Camera camera;
        private Light ambient;
        private List<GeometricObject> objects;
        private List<Light> lights;

        //observers
        List<ISceneObserver> observers;

        public ViewPlane ViewPlane
        {
            get { return vp; }
            set { vp = value; }
        }

        public Vec3 BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
            }
        }

        public Tracer Tracer
        {
            get { return tracer; }
            set { tracer = value; }
        }

        public Camera Camera
        {
            get { return camera; }
            set
            {
                camera = value;
            }
        }

        public Light AmbientLight
        {
            get { return ambient; }
            set
            {
                ambient = value;
            }
        }

        public List<GeometricObject> Objects
        {
            get { return objects; }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }

        public SceneBase()
        {
            objects = new List<GeometricObject>();
            lights = new List<Light>();
            observers = new List<ISceneObserver>();
        }
        
        public void AddObject(GeometricObject obj)
        {
            objects.Add(obj);
        }

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public virtual void DisplayPixel(int column, int row, Vec3 pixelColor)
        {
            NotifyObservers_DisplayPixel(column, row, pixelColor);
        }

        public abstract void Build();

        public virtual void RenderScene()
        {
            NotifyObservers_RayTracerStarted();
            camera.RenderScene(this);
            //camera.MultiThreadRenderScene(this);
            NotifyObservers_RayTracerFinished();
        }

        public void Show()
        {
            Build();
            RenderScene();
        }

        public ShadeRec HitBareBonesObjects(Ray ray)
        {
            ShadeRec sr = new ShadeRec(this);
            double t = 0.0;
            double tmin = MathUtils.HugeValue;
            int numObjects = objects.Count;

            foreach (var obj in objects)
            {
                if(obj.Hit(ray, ref t, sr) && (t < tmin))
                {
                    sr.HitAnObject = true;
                    tmin = t;
                    sr.Color = obj.Color;
                }
            }

            return sr;
        }

        public ShadeRec HitObjects(Ray ray)
        {
            ShadeRec sr = new ShadeRec(this);
            double t = 0.0;
            Vec3 normal = null;
            Vec3 lhPoint = null;
            double tmin = MathUtils.HugeValue;

            foreach (var obj in objects)
            {
                if (obj.Hit(ray, ref t, sr) && (t < tmin))
                {
                    sr.HitAnObject = true;
                    tmin = t;
                    sr.Material = obj.Material;
                    sr.HitPoint = ray.HitPoint(t);
                    normal = sr.Normal;
                    lhPoint = sr.LocalHitPoint;
                }
            }

            if (sr.HitAnObject)
            {
                sr.Tmin = tmin;
                sr.Normal = normal;
                sr.LocalHitPoint = lhPoint;
            }

            return sr;
        }

        public bool ShadowHitObjects(Ray ray, double d)
        {
            double t = 0.0;

            foreach (var obj in objects)
            {
                if (obj.ShadowHit(ray, ref t) && t < d)
                    return true;
            }

            return false;
        }

        public void RegisterObserver(ISceneObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(ISceneObserver observer)
        {
            observers.Remove(observer);
        }

        public void RemoveObserver(int id)
        {
            observers.RemoveAt(id);
        }

        public void NotifyObservers_RayTracerStarted()
        {
            foreach (var observer in observers)
            {
                observer.RayTracerStarted(vp.Hres, vp.Vres);
            }
        }

        public void NotifyObservers_DisplayPixel(int column, int row, Vec3 pixelColor)
        {
            int invR = vp.Vres - row - 1;
            int rgb = ColorUtils.ToRGB(ColorUtils.Saturate(pixelColor));

            foreach (var observer in observers)
            {
                observer.DisplayPixel(column, invR, rgb);
            }
        }

        public void NotifyObservers_RayTracerFinished()
        {
            foreach (var observer in observers)
            {
                observer.RayTracerFinished(this);
            }
        }

        public abstract string Name { get; }
    }

}
