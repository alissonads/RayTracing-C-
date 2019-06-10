using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.GeometricObjects.Composite
{
    class Compound : GeometricObject
    {
        protected List<GeometricObject> objects;

        public int NumObjects
        {
            get { return objects.Count; }
        }

        public override IMaterial Material
        {
            get { return material; }

            set
            {
                foreach (var obj in objects)
                {
                    obj.Material = value;
                }
            }
        }

        public Compound() :
            base()
        {
            objects = new List<GeometricObject>();
        }

        public Compound(Compound other) :
            base(other)
        {
            objects = other.objects.ToList();
        }

        public void AddObject(GeometricObject obj)
        {
            objects.Add(obj);
        }

        public void RemoveObject(GeometricObject obj)
        {
            objects.Remove(obj);
        }

        public void RemoveObject(int i)
        {
            objects.RemoveAt(i);
        }

        private void DeleteObjects()
        {
            objects.Clear();
        }

        public void CopyObjects(List<GeometricObject> objs)
        {
            DeleteObjects();
            objects = objs.ToList();
        }

        public override GeometricObject Clone()
        {
            return new Compound(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t = 0.0;
            Vec3 normal = null;
            Vec3 lhPoint = null;
            bool hit = false;
            tmin = MathUtils.HugeValue;

            foreach (var obj in objects)
            {
                if (obj.Hit(ray, ref t, sr) && (t < tmin))
                {
                    hit = true;
                    tmin = t;
                    material = obj.Material;
                    normal = sr.Normal;
                    lhPoint = sr.LocalHitPoint;
                }
            }

            if (hit)
            {
                sr.Tmin = tmin;
                sr.Normal = normal;
                sr.LocalHitPoint = lhPoint;
            }

            return hit;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double t = 0.0;
            bool hit = false;
            tmin = MathUtils.HugeValue;

            foreach (var obj in objects)
            {
                if (obj.ShadowHit(ray, ref t) && (t < tmin))
                {
                    hit = true;
                    tmin = t;
                }
            }

            return hit;
        }
    }
}
