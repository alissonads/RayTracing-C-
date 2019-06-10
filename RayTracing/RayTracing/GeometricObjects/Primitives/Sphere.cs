using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Primitives
{
    class Sphere : GeometricObject
    {
        private Vec3 center;
        private double radius;
        private static double kEpsilon = 0.1;

        public Vec3 Center
        {
            get { return center; }
            set { center = value; }
        }

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public override BBox BoundingBox
        {
            get
            {
                return new BBox(center - radius, center + radius);
            }
        }

        public Sphere() :
            base()
        {
            center = new Vec3();
            radius = 1.0;
        }

        public Sphere(Vec3 center, double radius) :
            this(center, radius, null)
        {}

        public Sphere(Vec3 center, double radius, IMaterial material) :
            base()
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public Sphere(Sphere other) :
            base(other)
        {
            center = other.center.Clone();
            radius = other.radius;
        }

        public void SetCenter(float x, float y, float z)
        {
            center.Set(x, y, z);
        }

        public override GeometricObject Clone()
        {
            return new Sphere(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            Vec3 temp = ray.O - center;

            double a = ray.D.Dot(ray.D);
            double b = 2.0 * temp.Dot(ray.D);
            double c = temp.SizeSQR - (radius * radius);

            double delta = (b * b) - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;
            double t = (-b - e) / denom;

            if(t > kEpsilon)
            {
                sr.Color = color;
                sr.LocalHitPoint = ray.HitPoint(t);
                sr.Normal = (temp + t * ray.D) / radius;
                tmin = t;
                return true;
            }

            t = (-b + e) / denom;

            if(t > kEpsilon)
            {
                sr.Color = color;
                sr.LocalHitPoint = ray.HitPoint(t);
                sr.Normal = (temp + t * ray.D) / radius;
                tmin = t;
                return true;
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            Vec3 temp = ray.O - center;

            double a = ray.D.Dot(ray.D);
            double b = 2.0 * temp.Dot(ray.D);
            double c = temp.SizeSQR - (radius * radius);

            double delta = (b * b) - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;
            double t = (-b - e) / denom;

            if (t > kEpsilon)
            {
                tmin = t;
                return true;
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                tmin = t;
                return true;
            }

            return false;
        }
    }
}
