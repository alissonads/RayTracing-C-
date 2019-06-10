using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BRDFs.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;
using RayTracing.Samplers;

namespace RayTracing.BRDFs
{
    class GlossySpecular : BRDF
    {
        private float ks;
        private Vec3 cs;
        private float exp;

        public float Ks
        {
            get { return ks; }
            set
            {
                ks = value;
            }
        }

        public Vec3 Cs
        {
            get { return cs; }
            set
            {
                cs = value;
            }
        }

        public float Exp
        {
            get { return exp; }
            set
            {
                exp = value;
            }
        }

        public GlossySpecular() :
            base()
        {
            ks = 0;
            cs = new Vec3(1);
            exp = 2;
        }

        public GlossySpecular(Sampler sampler) :
            this(0.0f, new Vec3(1.0), 2.0f, sampler)
        {}

        public GlossySpecular(float ks, Vec3 cs, float exp) :
            this(ks, cs, exp, null)
        {}

        public GlossySpecular(float ks, Vec3 cs, float exp, Sampler sampler) :
            base(sampler)
        {
            this.ks = ks;
            this.cs = cs;
            this.exp = exp;
        }

        public GlossySpecular(GlossySpecular other) : 
            base(other)
        {
            ks = other.ks;
            cs = other.cs.Clone();
            exp = other.exp;
        }

        public void SetCs(float r, float g, float b)
        {
            cs.Set(r, g, b);
        }

        public void SetCs(float rgb)
        {
            cs.Set(rgb);
        }

        public void SetSampler(Sampler sampler, float exp)
        {
            this.sampler = sampler;
            sampler.MapSamplesToHemisphere(exp);
        }

        public void SetSamples(int num, double exp)
        {
            sampler = new MultiJittered(num);
            sampler.MapSamplesToHemisphere(exp);
        }

        public override BRDF Clone()
        {
            return new GlossySpecular(this);
        }

        public override Vec3 F(ShadeRec sr, Vec3 wi, Vec3 wo)
        {
            Vec3 L = new Vec3();
            float ndotwi = (float)sr.Normal.Dot(wi);
            Vec3 r = (wi * -1) + 2.0 * sr.Normal * ndotwi;
            float rdotwo = (float)r.Dot(wo);

            if (rdotwo > 0.0)
                L = ks * cs * Math.Pow(rdotwo, exp);

            return L;
        }
        
        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo, ref float pdf)
        {
            float ndotwo = (float)sr.Normal.Dot(wo);
            Vec3 r = (wo * -1) + 2.0 * sr.Normal * ndotwo;

            Vec3 w = r;
            Vec3 u = new Vec3(0.00424, 1.0, 0.00764).Cross(w);
            u.Normalize();
            Vec3 v = Vec3.Cross(u, w);

            Vec3 sp = sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;

            if (sr.Normal.Dot(wi) < 0.0)
                wi = -sp.X * u - sp.Y * v + sp.Z * w;

            float phongLobe = (float)Math.Pow(r.Dot(wi), exp);
            pdf = (float)(phongLobe * sr.Normal.Dot(wi));

            return (ks * cs * phongLobe);
        }
    }
}
