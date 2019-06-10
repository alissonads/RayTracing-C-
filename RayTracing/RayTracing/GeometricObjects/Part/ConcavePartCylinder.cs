using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.GeometricObjects.Part
{
    class ConcavePartCylinder : GeometricObject
    {
        protected double y0;
        protected double y1;
        protected double radius;
        protected double invRadius;
        private double phiMin;
        private double phiMax;
        private static double kEpsilon = 0.0001;

        public ConcavePartCylinder() :
            this(-1, 1, 1)
        {}

        public ConcavePartCylinder(double y0, double y1, double radius) :
            this(y0, y1, radius, 0, 360)
        {}

        public ConcavePartCylinder(double y0, 
                                   double y1, 
                                   double radius, 
                                   double azimuthMin,
                                   double azimuthMax) :
            base()
        {
            this.y0 = y0;
            this.y1 = y1;
            this.radius = radius;
            invRadius = 1 / radius;
            phiMin = azimuthMin * MathUtils.TORAD;
            phiMax = azimuthMax * MathUtils.TORAD;
        }

        public ConcavePartCylinder(ConcavePartCylinder other) :
            base(other)
        {
            y0 = other.y0;
            y1 = other.y1;
            radius = other.radius;
            invRadius = other.invRadius;
            phiMin = other.phiMin;
            phiMax = other.phiMax;
        }

        public override GeometricObject Clone()
        {
            return new ConcavePartCylinder(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t;
            double ox = ray.O.X;
            double oy = ray.O.Y;
            double oz = ray.O.Z;
            double dx = ray.D.X;
            double dy = ray.D.Y;
            double dz = ray.D.Z;

            double a = dx * dx + dz * dz;
            double b = 2.0 * (ox * dx + oz * dz);
            double c = ox * ox + oz * oz - radius * radius;
            double delta = b * b - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;
            t = (-b - e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;
                Vec3 hit = ray.HitPoint(t);
                double phi = Math.Atan2(hit.X, hit.Z);

                if (phi < 0)
                {
                    phi += MathUtils.TwoPI;
                }

                if (yhit > y0 && yhit < y1 && phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    sr.Normal = new Vec3((ox + t * dx) * invRadius,
                                          0.0,
                                         (oz + t * dz) * invRadius).Neg();

                    //if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                    //    sr.Normal.Neg();

                    sr.LocalHitPoint = ray.HitPoint(t);

                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;
                Vec3 hit = ray.HitPoint(t);
                double phi = Math.Atan2(hit.X, hit.Z);

                if (phi < 0)
                {
                    phi += MathUtils.TwoPI;
                }

                if (yhit > y0 && yhit < y1 && phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    sr.Normal = new Vec3((ox + t * dx) * invRadius,
                                          0.0,
                                         (oz + t * dz) * invRadius).Neg();

                    //if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                    //    sr.Normal.Neg();

                    sr.LocalHitPoint = ray.HitPoint(t);

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

            double a = dx * dx + dz * dz;
            double b = 2.0 * (ox * dx + oz * dz);
            double c = ox * ox + oz * oz - radius * radius;
            double delta = b * b - 4.0 * a * c;

            if (delta < 0.0)
                return false;

            double e = Math.Sqrt(delta);
            double denom = 2.0 * a;
            t = (-b - e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;
                Vec3 hit = ray.HitPoint(t);
                double phi = Math.Atan2(hit.X, hit.Z);

                if (phi < 0)
                {
                    phi += MathUtils.TwoPI;
                }

                if (yhit > y0 && yhit < y1 && phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;
                Vec3 hit = ray.HitPoint(t);
                double phi = Math.Atan2(hit.X, hit.Z);

                if (phi < 0)
                {
                    phi += MathUtils.TwoPI;
                }

                if (yhit > y0 && yhit < y1 && phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    return true;
                }
            }

            return false;
        }
    }
}
