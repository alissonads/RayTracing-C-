using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Primitives
{
    class ConcaveSphere : GeometricObject, IEmissiveObject
    {
        private Vec3 center;
        private double radius;
        private float invArea;
        private static double kEpsilon = 0.001;
        private Sampler sampler;
        private IEmissiveMaterial emissive;

        public Vec3 Center
        {
            get { return center; }
            set
            {
                center = value;
            }
        }

        public double Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                invArea = (float)(1.0 / (4 * MathUtils.PI * value * value));
            }
        }

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                sampler.MapSamplesToSphere();
            }
        }

        public override BBox BoundingBox
        {
            get
            {
                return new BBox(center + -radius, center + radius);
            }
        }

        public IEmissiveMaterial EmissiveMaterial
        {
            get { return emissive; }

            set
            {
                emissive = value;
            }
        }

        public ConcaveSphere() :
            base()
        {
            center = new Vec3();
            radius = 1.0;
            invArea = (float)(1.0 / (4 * MathUtils.PI * radius * radius));
        }

        public ConcaveSphere(Vec3 center, double radius) :
            this(center, radius, null)
        {}

        public ConcaveSphere(Vec3 center, double radius,
                             IEmissiveMaterial emissive) :
            base()
        {
            this.center = center;
            this.radius = radius;
            invArea = (float)(1.0 / (4 * MathUtils.PI * radius * radius));
            this.emissive = emissive;
        }

        public ConcaveSphere(ConcaveSphere other) :
            base(other)
        {
            center = other.center.Clone();
            radius = other.radius;
            invArea = other.invArea;

            if (other.sampler != null)
                sampler = other.sampler.Clone();
            if (other.emissive != null)
                emissive = other.emissive.Clone();
        }

        public override GeometricObject Clone()
        {
            return new ConcaveSphere(this);
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

            if (t > kEpsilon)
            {
                sr.Color = color;
                sr.LocalHitPoint = ray.HitPoint(t);
                sr.Normal = (temp + t * ray.D) / -radius;
                tmin = t;
                return true;
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                sr.Color = color;
                sr.LocalHitPoint = ray.HitPoint(t);
                sr.Normal = (temp + t * ray.D) / -radius;
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

        public Vec3 GetNormal(Vec3 sp)
        {
            Vec3 n = center - sp;
            n.Normalize();
            return n.Neg();
        }

        public Vec3 Sample()
        {
            return sampler.SampleSphere() * radius + center;
        }

        public float PDF(ShadeRec sr)
        {
            return invArea;
        }

        public IEmissiveObject CloneEmissive()
        {
            return new ConcaveSphere(this);
        }
    }
}
