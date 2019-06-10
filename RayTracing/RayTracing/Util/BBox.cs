using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util
{
    class BBox
    {
        public double x0, y0, z0;
        public double x1, y1, z1;
        private static double kEpsilon = 0.0001;

        public BBox()
        {
            x0 = y0 = z0 = -1;
            x1 = y1 = z1 = 1;
        }

        public BBox(double x0, double x1, double y0,
                    double y1, double z0, double z1)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.z0 = z0;
            this.z1 = z1;
        }

        public BBox(Vec3 p0, Vec3 p1)
        {
            x0 = p0.X;
            y0 = p0.Y;
            z0 = p0.Z;
            x1 = p1.X;
            y1 = p1.Y;
            z1 = p1.Z;
        }

        public BBox(BBox other)
        {
            x0 = other.x0;
            x1 = other.x1;
            y0 = other.y0;
            y1 = other.y1;
            z0 = other.z0;
            z1 = other.z1;
        }

        public BBox Clone()
        {
            return new BBox(this);
        }

        public bool Hit(Ray ray)
        {
            double ox = ray.O.X; double oy = ray.O.Y; double oz = ray.O.Z;
            double dx = ray.D.X; double dy = ray.D.Y; double dz = ray.D.Z;

            double txMin, tyMin, tzMin;
            double txMax, tyMax, tzMax;

            double a = 1.0 / dx;
            if(a >= 0)
            {
                txMin = (x0 - ox) * a;
                txMax = (x1 - ox) * a;
            }
            else
            {
                txMin = (x1 - ox) * a;
                txMax = (x0 - ox) * a;
            }

            double b = 1.0 / dy;
            if (b >= 0)
            {
                tyMin = (y0 - oy) * b;
                tyMax = (y1 - oy) * b;
            }
            else
            {
                tyMin = (y1 - oy) * b;
                tyMax = (y0 - oy) * b;
            }

            double c = 1.0 / dz;
            if (c >= 0)
            {
                tzMin = (z0 - oz) * c;
                tzMax = (z1 - oz) * c;
            }
            else
            {
                tzMin = (z1 - oz) * c;
                tzMax = (z0 - oz) * c;
            }

            double t0, t1;

            //find largest entering t value

            if (txMin > tyMin)
                t0 = txMin;
            else
                t0 = tyMin;

            if (tzMin > t0)
                t0 = tzMin;

            // find smallest exiting t value

            if (txMax < tyMax)
                t1 = txMax;
            else
                t1 = tyMax;

            if (tzMax < t1)
                t1 = tzMax;

            return (t0 < t1 && t1 > kEpsilon);
        }

        public bool Inside(Vec3 p)
        {
            return ((p.X > x0 && p.X < x1) &&
                    (p.Y > y0 && p.Y < y1) &&
                    (p.Z > z0 && p.Z < z1)   );
        }
    }
}
