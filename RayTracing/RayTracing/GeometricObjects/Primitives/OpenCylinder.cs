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
    class OpenCylinder : Base.GeometricObject
    {
        private double y0, y1;
        private double radius;
        private double invRadius;
        private static double kEpsilon = 0.0001;

        public OpenCylinder() :
            this(-1.0, 1.0, 1.0)
        {}

        public OpenCylinder(double bottom, 
                            double top, 
                            double radius) :
            this(bottom, top, radius, null)
        {}

        public OpenCylinder(double bottom,
                            double top,
                            double radius,
                            IMaterial material) :
            base()
        {
            y0 = bottom;
            y1 = top;
            this.radius = radius;
            invRadius = 1.0 / radius;
            this.material = material;
        }

        public OpenCylinder(OpenCylinder other) :
            base(other)
        {
            y0 = other.y0;
            y1 = other.y1;
            radius = other.radius;
            invRadius = other.invRadius;
        }

        public override GeometricObject Clone()
        {
            return new OpenCylinder(this);
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

            if(t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if(yhit > y0 && yhit < y1)
                {
                    tmin = t;
                    sr.Normal = new Vec3((ox + t * dx) * invRadius,
                                         0.0, 
                                         (oz + t * dz) * invRadius);

                    if (ray.D.Clone().Neg().Dot(sr.Normal) < 0.0)
                        sr.Normal.Neg();

                    sr.LocalHitPoint = ray.HitPoint(t);

                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if (yhit > y0 && yhit < y1)
                {
                    tmin = t;
                    sr.Normal = new Vec3((ox + t * dx) * invRadius,
                                         0.0,
                                         (oz + t * dz) * invRadius);

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

                if (yhit > y0 && yhit < y1)
                {
                    tmin = t;

                    return true;
                }
            }

            t = (-b + e) / denom;

            if (t > kEpsilon)
            {
                double yhit = oy + t * dy;

                if (yhit > y0 && yhit < y1)
                {
                    tmin = t;

                    return true;
                }
            }

            return false;
        }
    }
}
