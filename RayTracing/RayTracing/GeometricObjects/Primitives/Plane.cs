using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.GeometricObjects.Base;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Primitives
{
    class Plane : GeometricObject
    {
        private Vec3 point;
        private Vec3 normal;
        private static double kEpsilon = 0.00001;

        public Vec3 Point
        {
            get { return point; }
        }

        public Vec3 Normal
        {
            get { return normal; }
        }
        
        public Plane() :
            base()
        {
            point = new Vec3();
            normal = new Vec3();
        }

        public Plane(Vec3 point, Vec3 normal) :
            this(point, normal, null)
        {}

        public Plane(Vec3 point, Vec3 normal, IMaterial material) :
            base()
        {
            this.point = point;
            this.normal = normal;
            this.material = material;
        }

        public Plane(Plane other) :
            base(other)
        {
            point = other.point.Clone();
            normal = other.normal.Clone();
        }
        
        public override GeometricObject Clone()
        {
            return new Plane(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t = (point - ray.O).Dot(normal) / ray.D.Dot(normal);

            if(t > kEpsilon)
            {
                sr.Color = color;
                sr.Normal = normal;
                sr.LocalHitPoint = ray.HitPoint(t);
                tmin = t;
                return true;
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            double t = (point - ray.O).Dot(normal) / ray.D.Dot(normal);

            if (t > kEpsilon)
            {
                tmin = t;
                return true;
            }

            return false;
        }
    }
}
