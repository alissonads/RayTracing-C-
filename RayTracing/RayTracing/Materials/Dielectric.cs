using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.BTDFs;
using RayTracing.BRDFs;
using RayTracing.Materials.Base;

namespace RayTracing.Materials
{
    class Dielectric : Phong
    {
        private Vec3 cfIn;
        private Vec3 cfOut;

        private FresnelReflector fresnelBrdf;
        private FresnelTransmitter fresnelBtdf;

        public FresnelReflector FresnelBrdf
        {
            get { return fresnelBrdf; }
            set
            {
                fresnelBrdf = value;
            }
        }

        public FresnelTransmitter FresnelBtdf
        {
            get { return fresnelBtdf; }
            set
            {
                fresnelBtdf = value;
            }
        }

        public Dielectric() :
            base()
        {
            cfIn = new Vec3(1);
            cfOut = new Vec3(1);
            fresnelBrdf = new FresnelReflector();
            fresnelBtdf = new FresnelTransmitter();
        }

        public Dielectric(Dielectric other) :
            base(other)
        {
            cfIn = other.cfIn.Clone();
            cfOut = other.cfOut.Clone();
            fresnelBrdf = (FresnelReflector)other.fresnelBrdf.Clone();
            fresnelBtdf = (FresnelTransmitter)other.fresnelBtdf.Clone();
        }

        public void SetEtaIn(float d)
        {
            fresnelBrdf.EtaIn = d;
            fresnelBtdf.EtaIn = d;
        }

        public void SetEtaOut(float d)
        {
            fresnelBrdf.EtaOut = d;
            fresnelBtdf.EtaOut = d;
        }

        public void SetCfIn(Vec3 cfIn)
        {
            this.cfIn = cfIn;
        }

        public void SetCfIn(float r, float g, float b)
        {
            cfIn.Set(r, g, b);
        }

        public void SetCfIn(float rgb)
        {
            cfIn.Set(rgb);
        }

        public void SetCfOut(Vec3 cfOut)
        {
            this.cfOut = cfOut;
        }

        public void SetCfOut(float r, float g, float b)
        {
            cfOut.Set(r, g, b);
        }

        public void SetCfOut(float rgb)
        {
            cfOut.Set(rgb);
        }

        public override IMaterial Clone()
        {
            return new Dielectric(this);
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 L = base.Shade(sr);

            Vec3 wi = null;
            Vec3 wo = sr.Ray.D * -1;
            Vec3 fr = fresnelBrdf.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);
            double t = MathUtils.HugeValue;
            Vec3 Lr = null;
            Vec3 Lt = null;
            float ndotwi = (float)sr.Normal.Dot(wi);


            if (fresnelBtdf.Tir(sr))
            {
                if (ndotwi < 0.0)
                {
                    Lr = sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1);
                    L += ColorUtils.Powc(cfIn, t) * Lr;
                }
                else
                {
                    Lr = sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1);
                    L += ColorUtils.Powc(cfOut, t) * Lr;
                }
            }
            else
            {
                Vec3 wt = null;
                Vec3 ft = fresnelBtdf.SampleF(sr, ref wt, wo);
                Ray transmittedRay = new Ray(sr.HitPoint, wt);
                float ndotwt = (float)sr.Normal.Dot(wt);

                if(ndotwi < 0.0)
                {
                    //reflected
                    Lr = fr * sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwi);
                    L += ColorUtils.Powc(cfIn, t) * Lr;

                    //transmitter
                    Lt = ft * sr.World.Tracer.TraceRay(transmittedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwt);
                    L += ColorUtils.Powc(cfOut, t) * Lt;
                }
                else
                {
                    //reflected
                    Lr = fr * sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwi);
                    L += ColorUtils.Powc(cfOut, t) * Lr;

                    //transmitter
                    Lt = ft * sr.World.Tracer.TraceRay(transmittedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwt);
                    L += ColorUtils.Powc(cfIn, t) * Lt;
                }
            }

            return L;
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 L = base.AreaLightShade(sr);

            Vec3 wi = null;
            Vec3 wo = sr.Ray.D * -1;
            Vec3 fr = fresnelBrdf.SampleF(sr, ref wi, wo);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);
            double t = MathUtils.HugeValue;
            Vec3 Lr = null;
            Vec3 Lt = null;
            float ndotwi = (float)sr.Normal.Dot(wi);


            if (fresnelBtdf.Tir(sr))
            {
                if (ndotwi < 0.0)
                {
                    Lr = sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1);
                    L += ColorUtils.Powc(cfIn, t) * Lr;
                }
                else
                {
                    Lr = sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1);
                    L += ColorUtils.Powc(cfOut, t) * Lr;
                }
            }
            else
            {
                Vec3 wt = null;
                Vec3 ft = fresnelBtdf.SampleF(sr, ref wt, wo);
                Ray transmittedRay = new Ray(sr.HitPoint, wt);
                float ndotwt = (float)sr.Normal.Dot(wt);

                if (ndotwi < 0.0)
                {
                    //reflected
                    Lr = fr * sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwi);
                    L += ColorUtils.Powc(cfIn, t) * Lr;

                    //transmitter
                    Lt = ft * sr.World.Tracer.TraceRay(transmittedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwt);
                    L += ColorUtils.Powc(cfOut, t) * Lt;
                }
                else
                {
                    //reflected
                    Lr = fr * sr.World.Tracer.TraceRay(reflectedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwi);
                    L += ColorUtils.Powc(cfOut, t) * Lr;

                    //transmitter
                    Lt = ft * sr.World.Tracer.TraceRay(transmittedRay, ref t, sr.Depth + 1)
                            * Math.Abs(ndotwt);
                    L += ColorUtils.Powc(cfIn, t) * Lt;
                }
            }

            return L;
        }
    }
}
