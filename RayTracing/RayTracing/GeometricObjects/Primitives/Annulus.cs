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
    class Annulus : GeometricObject
    {
        private Vec3 center;
        private Vec3 normal;
        private double outerRadius;
        private double innerRadius;
        private static double kEpsilon = 0.1;

        public Annulus() :
            this(new Vec3(),
                 new Vec3(0, 1, 0),
                 0.5, 1.0)
        {}

        public Annulus(Vec3 center, 
                       Vec3 normal, 
                       double innerRadius, 
                       double outerRadius) :
            this(center, normal, 
                innerRadius, 
                outerRadius, null)
        {}

        public Annulus(Vec3 center,
                       Vec3 normal,
                       double innerRadius,
                       double outerRadius,
                       IMaterial material) :
            base()
        {
            this.center = center;
            this.normal = normal;
            this.normal.Normalize();
            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.material = material;
        }

        public Annulus(Annulus other) :
            base(other)
        {
            center = other.center.Clone();
            normal = other.normal.Clone();
            innerRadius = other.innerRadius;
            outerRadius = other.outerRadius;
        }

        public override GeometricObject Clone()
        {
            return new Annulus(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            double t = (center - ray.O).Dot(normal) / ray.D.Dot(normal);

            if (t < kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);
            double dsqr = (center - p).SizeSQR;

            if ((dsqr >= outerRadius * outerRadius) || (dsqr <= innerRadius * innerRadius))
                return false;

            tmin = t;
            sr.Normal = normal;
            sr.LocalHitPoint = p;

            return true;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            double t = (center - ray.O).Dot(normal) / ray.D.Dot(normal);

            if (t < kEpsilon)
                return false;

            Vec3 p = ray.HitPoint(t);
            double dsqr = (center - p).SizeSQR;

            if ((dsqr >= outerRadius * outerRadius) || (dsqr <= innerRadius * innerRadius))
                return false;

            tmin = t;

            return true;
        }
        
    }
}
