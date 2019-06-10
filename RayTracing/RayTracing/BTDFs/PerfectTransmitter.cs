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
    class PerfectTransmitter : BTDF
    {
        private float kt;
        private float ior;

        public float Kt
        {
            get { return kt; }
            set
            {
                kt = value;
            }
        }

        public float Ior
        {
            get { return ior; }
            set
            {
                ior = value;
            }
        }

        public PerfectTransmitter() :
            base()
        {
            kt = 0;
            ior = 1;
        }

        public PerfectTransmitter(float kt, float ior) :
            base()
        {
            this.kt = kt;
            this.ior = ior;
        }

        public PerfectTransmitter(PerfectTransmitter other) :
            base()
        {
            kt = other.kt;
            ior = other.ior;
        }

        public override BTDF Clone()
        {
            return new PerfectTransmitter(this);
        }
        
        public override Vec3 SampleF(ShadeRec sr, ref Vec3 wt, Vec3 wo)
        {
            Vec3 n = sr.Normal.Clone();
            float cosTheta = (float)n.Dot(wo);
            float eta = ior;

            if(cosTheta < 0.0)
            {
                cosTheta = -cosTheta;
                n.Neg();
                eta = 1.0f / eta;
            }

            float temp = 1.0f - (1.0f - cosTheta * cosTheta) / (eta * eta);
            float cosTheta2 = (float)Math.Sqrt(temp);

            wt = wo.Clone().Neg() / eta - (cosTheta2 - cosTheta / eta) * n;


            return (kt / (eta * eta) * ColorUtils.WHITE / Math.Abs(sr.Normal.Dot(wt)));
        }

        public override bool Tir(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            float cosTheta = (float)sr.Normal.Dot(wo);
            float eta = ior;

            if (cosTheta < 0)
                eta = 1 / eta;

            return (1 - (1 - cosTheta * cosTheta) / (eta * eta)) < 0;
        }
    }
}
