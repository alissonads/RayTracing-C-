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
    class FresnelReflector : BRDF
    {
        private float etaIn;
        private float etaOut;

        public float EtaIn
        {
            get { return etaIn; }
            set
            {
                etaIn = value;
            }
        }

        public float EtaOut
        {
            get { return etaOut; }
            set
            {
                etaOut = value;
            }
        }

        public FresnelReflector() :
            base()
        {
            etaIn = 1;
            etaOut = 1;
        }

        public FresnelReflector(FresnelReflector other) :
            base(other)
        {
            etaIn = other.etaIn;
            etaOut = other.etaOut;
        }

        public override BRDF Clone()
        {
            return new FresnelReflector(this);
        }
        
        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wr, Vec3 wo)
        {
            float ndotwo = (float)sr.Normal.Dot(wo);
            wr = (wo * -1) + 2.0 * sr.Normal * ndotwo;

            return (Fresnel(sr) * ColorUtils.WHITE / Math.Abs(sr.Normal.Dot(wr)));
        }
        
        public float Fresnel(ShadeRec sr)
        {
            Vec3 normal = sr.Normal.Clone();
            float ndotd = (float)(normal*-1.0).Dot(sr.Ray.D);
            float eta;

            if (ndotd < 0.0f)
            {
                normal.Neg();
                eta = etaOut / etaIn;
            }
            else
                eta = etaIn / etaOut;

            float cosThetaI = (float)(normal * -1).Dot(sr.Ray.D);
            float cosThetaT = (float)Math.Sqrt(1.0 - (1.0 - cosThetaI * cosThetaI) / (eta * eta));
            float rParallel = (eta * cosThetaI - cosThetaT) / (eta * cosThetaI + cosThetaT);
            float rPerpendicular = (cosThetaI - eta * cosThetaT) / (cosThetaI + eta * cosThetaT);
            float kr = 0.5f * (rParallel * rParallel + rPerpendicular * rPerpendicular);

            return kr;
        }
    }
}
