using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BTDFs.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.BTDFs
{
    class FresnelTransmitter : BTDF
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

        public FresnelTransmitter() :
            base()
        {
            etaIn = 1;
            etaOut = 1;
        }

        public FresnelTransmitter(FresnelTransmitter other) :
            base()
        {
            etaIn = other.etaIn;
            etaOut = other.etaOut;
        }

        public override BTDF Clone()
        {
            return new FresnelTransmitter(this);
        }

        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wt, Vec3 wo)
        {
            Vec3 n = sr.Normal.Clone();
            float cosThetaI = (float)n.Dot(wo);
            float eta = etaIn / etaOut;

            if(cosThetaI < 0)
            {
                cosThetaI *= -1;
                n.Neg();
                eta = 1 / eta;
            }

            float temp = 1 - (1 - cosThetaI * cosThetaI) / (eta * eta);
            float cosTheta2 = (float)Math.Sqrt(temp);
            wt = (wo * -1) / eta - n * (cosTheta2 - cosThetaI / eta);

            return Fresnel(sr) / (eta * eta) * ColorUtils.WHITE / Math.Abs(sr.Normal.Dot(wt));
        }

        public override bool Tir(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            float cosThetaI = (float)sr.Normal.Dot(wo);
            float eta;
            float ndotd = (float)(sr.Normal * -1).Dot(sr.Ray.D);

            if (ndotd < 0.0)
                eta = etaOut / etaIn;
            else
                eta = etaIn / etaOut;

            return (1 - (1 - cosThetaI * cosThetaI) / (eta * eta)) < 0;
        }

        public float Fresnel(ShadeRec sr)
        {
            Vec3 normal = sr.Normal.Clone();
            float ndotd = (float)(normal * -1).Dot(sr.Ray.D);
            float eta;

            if (ndotd < 0.0)
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

            return (1 - kr);
        }
    }
}
