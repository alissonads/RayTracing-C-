using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BRDFs.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;

namespace RayTracing.BRDFs
{
    class Lambertian : BRDF
    {
        private float kd;
        Vec3 cd;

        public float Kd
        {
            get { return kd; }
            set { kd = value; }
        }

        public Vec3 Cd
        {
            get { return cd; }
            set { cd = value; }
        }

        public Lambertian() :
            this(0.0f, new Vec3())
        {}

        public Lambertian(Sampler sampler) :
            this(0.0f, new Vec3(), sampler)
        {}

        public Lambertian(float kd, Vec3 cd) :
            this(kd, cd, null)
        {}

        public Lambertian(float kd, Vec3 cd, Sampler sampler) :
            base(sampler)
        {
            this.kd = kd;
            this.cd = cd;
        }

        public Lambertian(Lambertian other) : 
            base(other)
        {
            kd = other.kd;
            cd = other.cd.Clone();
        }

        public void SetCd(float r, float g, float b)
        {
            cd.Set(r, g, b);
        }

        public void SetCd(float rgb)
        {
            cd.Set(rgb);
        }

        public override BRDF Clone()
        {
            return new Lambertian(this);
        }

        public override Vec3 F(ShadeRec sr, Vec3 wi, Vec3 wo)
        {
            return (kd * cd * MathUtils.InvPI);
        }
        
        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo, ref float pdf)
        {
            Vec3 w = sr.Normal;
            Vec3 v = Vec3.Cross(new Vec3(0.0034, 1.0, 0.0071), w);
            v.Normalize();
            Vec3 u = Vec3.Cross(v, w);

            Vec3 sp = sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;
            wi.Normalize();

            pdf = (float)(sr.Normal.Dot(wi) * MathUtils.InvPI);

            return kd * cd * MathUtils.InvPI;
        }

        public override Vec3 RHO(ShadeRec sr, Vec3 wo)
        {
            return (kd * cd);
        }
    }
}
