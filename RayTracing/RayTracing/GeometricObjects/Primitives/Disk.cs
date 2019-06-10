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
    class Disk : GeometricObject
    {
        private Vec3 center;
        private Vec3 normal;
        private double radius;
        private static double kEpsilon = 0.0001;

        public Vec3 Center
        {
            get { return center; }
            set
            {
                center = value;
            }
        }

        public Vec3 Normal
        {
            get { return normal; }
            set
            {
                normal = value;
            }
        }

        public double Radius
        {
            get { return radius; }
            set
            {
                radius = value;
            }
        }

        public Disk() :
            base()
        {
            center = new Vec3();
            normal = new Vec3(0, 1, 0);
            radius = 1.0;
        }

        public Disk(Vec3 center, Vec3 normal, double radius) :
            this(center, normal, radius, null)
        {}

        public Disk(Vec3 center, Vec3 normal, double radius, IMaterial material) :
            base()
        {
            this.center = center;
            this.normal = normal;
            this.normal.Normalize();
            this.radius = radius;
            this.material = material;
        }

        public Disk(Disk other) :
            base(other)
        {
            center = other.center.Clone();
            normal = other.normal.Clone();
            radius = other.radius;
        }

        public override GeometricObject Clone()
        {
            return new Disk(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t = (center - ray.O).Dot(normal) / ray.D.Dot(normal);

            if (t < kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);

            if ((center - p).SizeSQR < (radius * radius))
            {
                tmin = t;
                sr.Normal = normal;
                sr.LocalHitPoint = p;
                return true;
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double t = (center - ray.O).Dot(normal) / ray.D.Dot(normal);

            if (t < kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);

            if ((center - p).SizeSQR < (radius * radius))
            {
                tmin = t;
                return true;
            }

            return false;
        }
    }
}
