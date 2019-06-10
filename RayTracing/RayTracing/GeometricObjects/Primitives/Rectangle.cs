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
    class Rectangle : GeometricObject
    {
        private Vec3 p0;
        private Vec3 a;
        private Vec3 b;
        private Vec3 normal;
        private double aLenSquared;
        private double bLenSquared;

        private static double kEpsilon = 0.1;

        public Vec3 P0
        {
            get { return p0; }
            set
            {
                p0 = value;
            }
        }

        public Vec3 A
        {
            get { return a; }
            set
            {
                a = value;
            }
        }

        public Vec3 B
        {
            get { return b; }
            set
            {
                b = value;
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
        
        public Rectangle() :
            base()
        {
            p0 = new Vec3(-1, 0, -1);
            a = new Vec3(0, 0, 2);
            b = new Vec3(2, 0, 0);
            normal = new Vec3(0, 1, 0);
            aLenSquared = bLenSquared = 4.0;
        }

        public Rectangle(Rectangle other) :
            base(other)
        {
            p0 = other.p0.Clone();
            a = other.a.Clone();
            b = other.b.Clone();
            normal = other.normal.Clone();
            aLenSquared = other.aLenSquared;
            bLenSquared = other.bLenSquared;
        }

        public Rectangle(Vec3 p0,
                         Vec3 a,
                         Vec3 b) :
            this(p0, a, b, Vec3.Cross(a, b).Normalize())
        {}

        public Rectangle(Vec3 p0, 
                         Vec3 a, 
                         Vec3 b,
                         Vec3 normal) :
            this(p0, a, b, normal, null)
        {}

        public Rectangle(Vec3 p0,
                         Vec3 a,
                         Vec3 b,
                         Vec3 normal,
                         IMaterial material) :
            base()
        {
            this.p0 = p0;
            this.a = a;
            this.b = b;
            this.normal = normal.Normalize();
            aLenSquared = a.SizeSQR;
            bLenSquared = b.SizeSQR;
            this.material = material;
        }

        public override GeometricObject Clone()
        {
            return new Rectangle(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t = (p0 - ray.O).Dot(normal) /
                        ray.D.Dot(normal);

            if (t <= kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);
            Vec3 d = p - p0;

            double ddota = d.Dot(a);
            if (ddota < 0.0 || ddota > aLenSquared)
                return false;

            double ddotb = d.Dot(b);
            if (ddotb < 0.0 || ddotb > bLenSquared)
                return false;

            sr.Color = color;
            sr.Normal = normal;
            sr.LocalHitPoint = p;
            tmin = t;

            return true;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double t = (p0 - ray.O).Dot(normal) /
                        ray.D.Dot(normal);

            if (t <= kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);
            Vec3 d = p - p0;

            double ddota = d.Dot(a);
            if (ddota < 0.0 || ddota > aLenSquared)
                return false;

            double ddotb = d.Dot(b);
            if (ddotb < 0.0 || ddotb > bLenSquared)
                return false;

            tmin = t;

            return true;
        }
    }
}
