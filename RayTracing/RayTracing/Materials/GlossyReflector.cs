using RayTracing.BRDFs;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util;
using RayTracing.Materials.Base;

namespace RayTracing.Materials
{
    class GlossyReflector : Phong
    {
        private GlossySpecular glossySpecular;

        public GlossySpecular GlossySpecular
        {
            get { return glossySpecular; }
            set
            {
                glossySpecular = value;
            }
        }

        public GlossyReflector() :
            base()
        {
            glossySpecular = new GlossySpecular();
        }

        public GlossyReflector(Vec3 color, 
                               float ka, 
                               float kd, 
                               float ks, 
                               float exp,
                               float kr,
                               float gsExp) :
            base(color, ka, kd, ks, exp)
        {
            glossySpecular = new GlossySpecular(kr, color, gsExp);
        }

        public GlossyReflector(GlossyReflector other) :
            base(other)
        {
            glossySpecular = (GlossySpecular)other.glossySpecular.Clone();
        }

        public override IMaterial Clone()
        {
            return new GlossyReflector(this);
        }

        public void SetSamples(int numSamples, float exp)
        {
            glossySpecular.SetSamples(numSamples, exp);
        }

        public void SetKr(float kr)
        {
            glossySpecular.Ks = kr;
        }

        public void SetExponent(float exp)
        {
            glossySpecular.Exp = exp;
        }

        public void SetCr(Vec3 color)
        {
            glossySpecular.Cs = color;
        }

        public void SetReflectiveColor(float r, float g, float b)
        {
            glossySpecular.Cs.Set(r, g, b);
        }

        public void SetCr(float rgb)
        {
            glossySpecular.Cs.Set(rgb);
        }

        public override void SetColor(Vec3 color)
        {
            base.SetColor(color);
            SetCr(color);
        }

        public override void SetColor(float r, float g, float b)
        {
            base.SetColor(r, g, b);
            SetReflectiveColor(r, g, b);
        }
        
        public override void SetColor(float rgb)
        {
            base.SetColor(rgb);
            SetCr(rgb);
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 L = base.Shade(sr);
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 fr = glossySpecular.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1) *
                 sr.Normal.Dot(wi) / pdf;

            return L;
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 L = base.AreaLightShade(sr);
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 fr = glossySpecular.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1) *
                 sr.Normal.Dot(wi) / pdf;

            return L;
        }

        public override Vec3 PathShade(ShadeRec sr)
        {
            Vec3 L = base.PathShade(sr);
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 f = glossySpecular.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                   * sr.Normal.Dot(wi) / pdf;

            return L;
        }

        public override Vec3 GlobalShade(ShadeRec sr)
        {
            Vec3 L = base.GlobalShade(sr);
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 f = glossySpecular.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            L += f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                   * sr.Normal.Dot(wi) / pdf;

            return L;
        }
    }
}
