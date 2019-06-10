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
    class OpenCone : GeometricObject
    {
        private double h;
        private double r;
        private Vec3 center;
        private static double kEpsilon = 0.001;

        public double H
        {
            get { return h; }
            set
            {
                h = value;
            }
        }

        public double R
        {
            get { return r; }
            set
            {
                r = value;
            }
        }

        public Vec3 Center
        {
            get { return center; }
            set
            {
                center = value;
            }
        }

        public OpenCone() :
            this(2, 1)
        {}

        public OpenCone(double h, double r) :
            this(new Vec3(), h, r)
        {}

        public OpenCone(Vec3 center, double h, double r) :
            this(center, h, r, null)
        {}

        public OpenCone(Vec3 center, double h, double r, IMaterial material) :
            base()
        {
            this.center = center;
            this.h = h;
            this.r = r;
            this.material = material;
        }

        public OpenCone(OpenCone other) :
            base(other)
        {
            center = other.center.Clone();
            h = other.h;
            r = other.r;
        }

        public override GeometricObject Clone()
        {
            return new OpenCone(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            Vec3 temp = ray.O - center;
            double t;

            //double ox = ray.O.X;
            //double oy = ray.O.Y;
            //double oz = ray.O.Z;
            //double ox = -temp.X;
            double ox = temp.X;
            double oy = temp.Y;
            double oz = temp.Z;

            double dx = ray.D.X;
            double dy = ray.D.Y;
            double dz = ray.D.Z;

            double r2 = r * r;
            double h2 = h * h;

            double u = dx * dx;
            double v = dy * dy;
            double w = dz * dz;
            double x = ox * ox;
            double y = oy * oy;
            double z = oz * oz;

            double a = ((h2 * u) / r2 + (h2 * w) / r2 - v);
            double b = (2 * h2 * ox * dx) / r2 + (2 * h2 * dz * oz) / r2 + 2 * h * dy - 2 * dy * oy;
            double c = (h2 * x) / r2 + (h2 * z) / r2 - h2 + 2 * h * oy - y;

            double delta = b * b - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;

            t = (-b - e) / denom;

            if(t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if(yhit >= 0 && yhit <= h)
                {
                    tmin = t;
                    sr.LocalHitPoint = ray.HitPoint(t);
                    sr.Normal = ComputeNormal(sr.LocalHitPoint);

                    if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                        sr.Normal.Neg();
                    
                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if (yhit >= 0 && yhit <= h)
                {
                    tmin = t;
                    sr.LocalHitPoint = ray.HitPoint(t);
                    sr.Normal = ComputeNormal(sr.LocalHitPoint);

                    if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                        sr.Normal.Neg();
                    
                    return true;
                }
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double t;

            double ox = ray.O.X;
            double oy = ray.O.Y;
            double oz = ray.O.Z;

            double dx = ray.D.X;
            double dy = ray.D.Y;
            double dz = ray.D.Z;

            double r2 = r * r;
            double h2 = h * h;

            double u = dx * dx;
            double v = dy * dy;
            double w = dz * dz;
            double x = ox * ox;
            double y = oy * oy;
            double z = oz * oz;

            double a = ((h2 * u) / r2 + (h2 * w) / r2 - v);
            double b = (2 * h2 * ox * dx) / r2 + (2 * h2 * dz * oz) / r2 + 2 * h * dy - 2 * dy * oy;
            double c = (h2 * x) / r2 + (h2 * z) / r2 - h2 + 2 * h * oy - y;

            double delta = b * b - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;

            t = (-b - e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if (yhit >= 0 && yhit <= h)
                {
                    tmin = t;
                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if (yhit >= 0 && yhit <= h)
                {
                    tmin = t;
                    return true;
                }
            }

            return false;
        }

        public Vec3 ComputeNormal(Vec3 p)
        {
            Vec3 normal = new Vec3(h * p.X / r,
                                 -(p.Y - h),
                                   h * p.Z / r);
            normal.Normalize();

            return normal;
        }
    }
}
