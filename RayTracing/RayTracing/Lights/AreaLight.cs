using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Lights.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Base;
using RayTracing.Materials.Base;

namespace RayTracing.Lights
{
    class AreaLight : Light
    {
        private IEmissiveObject obj;
        private Vec3 samplePoint;
        private Vec3 normal;
        private Vec3 wi;

        public AreaLight() :
            base()
        {}

        public AreaLight(IEmissiveObject obj) :
            base()
        {
            this.obj = obj;
        }

        public AreaLight(AreaLight other) :
            base(other)
        {
            Object = other.obj.CloneEmissive();
        }

        public IEmissiveObject Object
        {
            private get { return obj; }
            set
            {
                obj = value;
            }
        }

        public override Light Clone()
        {
            return new AreaLight(this);
        }

        public override Vec3 GetDirection(ShadeRec sr)
        {
            samplePoint = obj.Sample();
            normal = obj.GetNormal(samplePoint);
            wi = samplePoint - sr.HitPoint;
            wi.Normalize();

            return wi;
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float ts = (float)(samplePoint - ray.O).Dot(ray.D);

            return sr.World.ShadowHitObjects(ray, ts);
        }

        public override Vec3 L(ShadeRec sr)
        {
            float ndotd = (float)(normal * -1).Dot(wi);

            return (ndotd > 0)?
                    obj.EmissiveMaterial.GetLe(sr) :
                    ColorUtils.BLACK;
        }

        public override double G(ShadeRec sr)
        {
            float ndotd = (float)(normal * -1).Dot(wi);
            float d = (float)(samplePoint - sr.HitPoint).SizeSQR;

            return ndotd / d;
        }

        public override float PDF(ShadeRec sr)
        {
            return obj.PDF(sr);
        }
    }
}
