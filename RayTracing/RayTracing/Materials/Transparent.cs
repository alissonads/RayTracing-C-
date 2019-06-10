using RayTracing.BRDFs;
using RayTracing.BTDFs;
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
    class Transparent : Phong
    {
        private PerfectSpecular reflective;
        private PerfectTransmitter specularBtdf;

        public PerfectSpecular Reflective
        {
            get { return reflective; }
            set
            {
                reflective = value;
            }
        }

        public PerfectTransmitter SpecularBtdf
        {
            get { return specularBtdf; }
            set
            {
                specularBtdf = value;
            }
        }

        public Transparent() :
            base()
        {
            reflective = new PerfectSpecular();
            specularBtdf = new PerfectTransmitter();
        }

        public Transparent(Vec3 color, 
                           float ka, 
                           float kd, 
                           float ks, 
                           float exp,
                           float kr,
                           float kt,
                           float ior) :
            base(color, ka, kd, ks, exp)
        {
            reflective = new PerfectSpecular(kr, color);
            specularBtdf = new PerfectTransmitter(kt, ior);
        }

        public Transparent(Transparent other) :
            base()
        {
            reflective = (PerfectSpecular)other.reflective.Clone();
            specularBtdf = (PerfectTransmitter)other.specularBtdf.Clone();
        }

        public override IMaterial Clone()
        {
            return new Transparent(this);
        }

        public void SetKr(float kr)
        {
            reflective.Kr = kr;
        }

        public void SetKt(float kt)
        {
            specularBtdf.Kt = kt;
        }

        public void SetIor(float ior)
        {
            specularBtdf.Ior = ior;
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 L = base.Shade(sr);

            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            Vec3 fr = reflective.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            if (specularBtdf.Tir(sr))
            {
                L += sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1);
            }
            else
            {
                Vec3 wt = null;
                Vec3 ft = specularBtdf.SampleF(sr, ref wt, wo);
                Ray transmittedRay = new Ray(sr.HitPoint, wt);

                L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                        * Math.Abs(sr.Normal.Dot(wi));

                L += ft * sr.World.Tracer.TraceRay(transmittedRay, sr.Depth + 1)
                       * Math.Abs(sr.Normal.Dot(wt));
            }

            return L;
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 L = base.AreaLightShade(sr);

            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            Vec3 fr = reflective.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            if (specularBtdf.Tir(sr))
            {
                L += sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1);
            }
            else
            {
                Vec3 wt = null;
                Vec3 ft = specularBtdf.SampleF(sr, ref wt, wo);
                Ray transmittedRay = new Ray(sr.HitPoint, wt);

                L += fr * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                        * Math.Abs(sr.Normal.Dot(wi));

                L += ft * sr.World.Tracer.TraceRay(transmittedRay, sr.Depth + 1)
                       * Math.Abs(sr.Normal.Dot(wt));
            }

            return L;
        }
    }
}
