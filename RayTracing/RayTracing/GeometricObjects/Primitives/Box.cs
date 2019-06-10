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
    class Box : GeometricObject
    {
        private double x0, y0, z0;
        private double x1, y1, z1;
        private static double kEpsilon = 0.001;

        public override BBox BoundingBox
        {
            get { return new BBox(x0, x1, y0, y1, z0, z1); }
        }

        public Box() :
            base()
        {
            x0 = y0 = z0 = -1;
            x1 = y1 = z1 = 1;
        }

        public Box(double x0, double x1, double y0,
                   double y1, double z0, double z1) :
            this(x0, x1, y0, y1, z0, z1, null)
        {}

        public Box(double x0, double x1, double y0,
                   double y1, double z0, double z1,
                   IMaterial material) :
            base()
        {
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.z0 = z0;
            this.z1 = z1;
            this.material = material;
        }

        public Box(Vec3 p0, Vec3 p1) :
            this(p0, p1, null)
        {}

        public Box(Vec3 p0, Vec3 p1, IMaterial material)
        {
            x0 = p0.X;
            y0 = p0.Y;
            z0 = p0.Z;
            x1 = p1.X;
            y1 = p1.Y;
            z1 = p1.Z;
            this.material = material;
        }

        public Box(Box other) :
            base(other)
        {
            x0 = other.x0;
            x1 = other.x1;
            y0 = other.y0;
            y1 = other.y1;
            z0 = other.z0;
            z1 = other.z1;
        }

        public override GeometricObject Clone()
        {
            return new Box(this);
        }


        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double ox = ray.O.X; double oy = ray.O.Y; double oz = ray.O.Z;
            double dx = ray.D.X; double dy = ray.D.Y; double dz = ray.D.Z;

            double txMin, tyMin, tzMin;
            double txMax, tyMax, tzMax;

            double a = 1.0 / dx;
            if (a >= 0)
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

            int faceIn, faceOut;

            //find largest entering t value

            if (txMin > tyMin)
            {
                t0 = txMin;
                faceIn = (a >= 0.0) ? 0 : 3;
            }
            else
            {
                t0 = tyMin;
                faceIn = (b >= 0.0) ? 1 : 4;
            }

            if (tzMin > t0)
            {
                t0 = tzMin;
                faceIn = (c >= 0.0) ? 2 : 5;
            }

            // find smallest exiting t value

            if (txMax < tyMax)
            {
                t1 = txMax;
                faceOut = (a >= 0.0) ? 3 : 0;
            }
            else
            {
                t1 = tyMax;
                faceOut = (b >= 0.0) ? 4 : 1;
            }

            if (tzMax < t1)
            {
                t1 = tzMax;
                faceOut = (c >= 0.0) ? 5 : 2;
            }

            if (t0 < t1 && t1 > kEpsilon)
            {
                if(t0 > kEpsilon)
                {
                    tmin = t0;
                    sr.Normal = GetNormal(faceIn);
                }
                else
                {
                    tmin = t1;
                    sr.Normal = GetNormal(faceOut);
                }
                
                sr.LocalHitPoint = ray.HitPoint(tmin);
                return true;
            }

            return false;
        }
        
        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double ox = ray.O.X; double oy = ray.O.Y; double oz = ray.O.Z;
            double dx = ray.D.X; double dy = ray.D.Y; double dz = ray.D.Z;

            double txMin, tyMin, tzMin;
            double txMax, tyMax, tzMax;

            double a = 1.0 / dx;
            if (a >= 0)
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

            if (t0 < t1 && t1 > kEpsilon)
            {
                if (t0 > kEpsilon)
                    tmin = t0;
                else
                    tmin = t1;

                return true;
            }

            return false;
        }

        private Vec3 GetNormal(int faceHit)
        {
            switch (faceHit)
            {
                case 0: return new Vec3(-1, 0, 0);
                case 1: return new Vec3(0, -1, 0);
                case 2: return new Vec3(0, 0, -1);
                case 3: return new Vec3(1, 0, 0);
                case 4: return new Vec3(0, 1, 0);
                case 5: return new Vec3(0, 0, 1);
            }

            return new Vec3();
        }
    }
}
