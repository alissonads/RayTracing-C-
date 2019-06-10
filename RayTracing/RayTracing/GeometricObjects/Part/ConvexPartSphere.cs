using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Part
{
    class ConvexPartSphere : GeometricObject
    {
        private Vec3 center;
        private double radius;
        private double phiMin;
        private double phiMax;
        private double thetaMin;
        private double thetaMax;
        
        private double cosThetaMin;
        private double cosThetaMax;


        private static double kEpsilon = 0.1;

        public ConvexPartSphere() :
            base()
        {
            center = new Vec3();
            radius = 1.0;
            phiMin = 0.0;
            phiMax = MathUtils.TwoPI;
            thetaMin = 0.0;
            thetaMax = MathUtils.PI;
            cosThetaMin = 1.0;
            cosThetaMax = -1.0;
        }

        public ConvexPartSphere(Vec3 c, double r,
                                double azimuthMin,
                                double azimuthMax,
                                double polarMin,
                                double polarMax) :
            this(c, r, azimuthMin, azimuthMax, 
                 polarMin, polarMax, null)
        {}

        public ConvexPartSphere(Vec3 c, double r,
                                double azimuthMin,
                                double azimuthMax,
                                double polarMin,
                                double polarMax,
                                IMaterial material) :
            base()
        {
            center = c;
            radius = r;
            phiMin = azimuthMin * MathUtils.TORAD;
            phiMax = azimuthMax * MathUtils.TORAD;
            thetaMin = polarMin * MathUtils.TORAD;
            thetaMax = polarMax * MathUtils.TORAD;
            cosThetaMin = Math.Cos(thetaMin);
            cosThetaMax = Math.Cos(thetaMax);
            this.material = material;
        }

        public ConvexPartSphere(Vec3 c, double r) :
            base()
        {
            center = c;
            radius = r;
            phiMin = 0.0;
            phiMax = MathUtils.TwoPI;
            thetaMin = 0.0;
            thetaMax = MathUtils.PI;
            cosThetaMin = 1.0;
            cosThetaMax = -1.0;
        }

        public ConvexPartSphere(ConvexPartSphere other) :
            base(other)
        {
            center = other.center.Clone();
            radius = other.radius;
            phiMin = other.phiMin;
            phiMax = other.phiMax;
            thetaMin = other.thetaMin;
            thetaMax = other.thetaMax;
            cosThetaMin = other.cosThetaMin;
            cosThetaMax = other.cosThetaMax;
        }

        public override GeometricObject Clone()
        {
            return new ConvexPartSphere(this);
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
                Vec3 hit = ray.HitPoint(t) - center;

                double phi = Math.Atan2(hit.X, hit.Z);
                if (phi < 0.0)
                    phi += MathUtils.TwoPI;

                if (hit.Y <= radius * cosThetaMin &&
                    hit.Y >= radius * cosThetaMax &&
                    phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    sr.Normal = (temp + t * ray.D) / radius;

                    if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                        sr.Normal.Neg();

                    sr.LocalHitPoint = ray.HitPoint(t);

                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                Vec3 hit = ray.HitPoint(t) - center;

                double phi = Math.Atan2(hit.X, hit.Z);
                if (phi < 0.0)
                    phi += MathUtils.TwoPI;

                if (hit.Y <= radius * cosThetaMin &&
                    hit.Y >= radius * cosThetaMax &&
                    phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    sr.Color = color;
                    sr.Normal = (temp + t * ray.D) / radius;

                    if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                        sr.Normal.Neg();

                    sr.LocalHitPoint = ray.HitPoint(t);

                    return true;
                }
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
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
                Vec3 hit = ray.HitPoint(t) - center;

                double phi = Math.Atan2(hit.X, hit.Z);
                if (phi < 0.0)
                    phi += MathUtils.TwoPI;

                if (hit.Y <= radius * cosThetaMin &&
                    hit.Y >= radius * cosThetaMax &&
                    phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                Vec3 hit = ray.HitPoint(t) - center;

                double phi = Math.Atan2(hit.X, hit.Z);
                if (phi < 0.0)
                    phi += MathUtils.TwoPI;

                if (hit.Y <= radius * cosThetaMin &&
                    hit.Y >= radius * cosThetaMax &&
                    phi >= phiMin && phi <= phiMax)
                {
                    tmin = t;
                    return true;
                }
            }

            return false;
        }
    }
}
