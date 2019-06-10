using RayTracing.BRDFs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.Materials
{
    class Reflective : Phong
    {
        private PerfectSpecular reflectiveBrdf;

        public PerfectSpecular ReflectiveBrdf
        {
            get { return reflectiveBrdf; }
            set
            {
                reflectiveBrdf = value;
            }
        }

        public Reflective() :
            base()
        {
            reflectiveBrdf = new PerfectSpecular();
        }

        public Reflective(Vec3 color, float ka, float kd, float ks, float exp, float kr) :
            base(color, ka, kd, ks, exp)
        {
            reflectiveBrdf = new PerfectSpecular(kr, color);
        }

        public Reflective(Reflective other) :
            base(other)
        {
            reflectiveBrdf = (PerfectSpecular)other.reflectiveBrdf.Clone();
        }

        public override IMaterial Clone()
        {
            return new Reflective(this);
        }

        public void SetKr(float kr)
        {
            reflectiveBrdf.Kr = kr;
        }

        public void SetReflectiveColor(Vec3 color)
        {
            reflectiveBrdf.Cr = color;
        }

        public void SetReflectiveColor(float r, float g, float b)
        {
            reflectiveBrdf.Cr.Set(r, g, b);
        }

        public void SetReflectiveColor(float rgb)
        {
            reflectiveBrdf.Cr.Set(rgb);
        }

        public override void SetColor(float r, float g, float b)
        {
            base.SetColor(r, g, b);
            SetReflectiveColor(r, g, b);
        }

        public override void SetColor(Vec3 color)
        {
            base.SetColor(color);
            SetReflectiveColor(color);
        }

        public override void SetColor(float rgb)
        {
            base.SetColor(rgb);
            SetReflectiveColor(rgb);
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 L = base.Shade(sr);

            Vec3 wo = (sr.Ray.D * -1);
            Vec3 wi = null;
            Vec3 fr = reflectiveBrdf.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1) *
                 sr.Normal.Dot(wi);

            return L;
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 L = base.AreaLightShade(sr);

            Vec3 wo = (sr.Ray.D * -1);
            Vec3 wi = null;
            Vec3 fr = reflectiveBrdf.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1) *
                 sr.Normal.Dot(wi);

            return L;
        }

        public override Vec3 PathShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0.0f;
            Vec3 f = reflectiveBrdf.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            return (f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                      * sr.Normal.Dot(wi) / pdf);
        }

        public override Vec3 GlobalShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 f = reflectiveBrdf.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            if(sr.Depth == 0)
                return f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 2)
                         * sr.Normal.Dot(wi) / pdf;
            
            return f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                     * sr.Normal.Dot(wi) / pdf;
        }
    }
}
