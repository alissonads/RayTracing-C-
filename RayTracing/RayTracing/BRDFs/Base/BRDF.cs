using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;
using RayTracing.Util;

namespace RayTracing.BRDFs.Base
{
    abstract class BRDF
    {
        protected Sampler sampler;

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                if (sampler != null)
                    sampler.MapSamplesToHemisphere(1);
            }
        }

        public BRDF()
        {}

        public BRDF(Sampler sampler)
        {
            this.sampler = sampler;
        }

        public BRDF(BRDF other)
        {
            if(sampler != null)
                sampler = other.sampler.Clone();
        }

        public abstract BRDF Clone();

        public virtual Vec3 F(ShadeRec sr, Vec3 wi, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 SampleF(ShadeRec sr, ref Vec3 wi, Vec3 wo, ref float pdf)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 RHO(ShadeRec sr, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }
    }
}
