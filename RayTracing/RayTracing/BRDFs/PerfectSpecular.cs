using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BRDFs.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.BRDFs
{
    class PerfectSpecular : BRDF
    {
        private float kr;
        private Vec3 cr;

        public float Kr
        {
            get { return kr; }
            set
            {
                kr = value;
            }
        }

        public Vec3 Cr
        {
            get { return cr; }
            set
            {
                cr = value;
            }
        }

        public PerfectSpecular() :
            this(0, new Vec3(1.0))
        {}

        public PerfectSpecular(float kr, Vec3 cr) :
            base()
        {
            this.kr = kr;
            this.cr = cr;
        }

        public PerfectSpecular(PerfectSpecular other) :
            base(other)
        {
            kr = other.kr;
            cr = other.cr.Clone();
        }

        public void SetCr(float r, float g, float b)
        {
            cr.Set(r, g, b);
        }

        public void SetCr(float rgb)
        {
            cr.Set(rgb);
        }

        public override BRDF Clone()
        {
            return new PerfectSpecular(this);
        }
        
        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo)
        {
            double ndotwo = sr.Normal.Dot(wo);
            wi = (wo * -1) + 2.0 * sr.Normal * ndotwo;
            return (kr * cr / Math.Abs(sr.Normal.Dot(wi)));
        }

        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo, ref float pdf)
        {
            float ndotwo = (float)sr.Normal.Dot(wo);
            Vec3 r = (wo * -1) + 2.0 * sr.Normal * ndotwo;
            pdf = (float)Math.Abs(sr.Normal.Dot(wi));

            return (kr * cr);
        }
    }
}
