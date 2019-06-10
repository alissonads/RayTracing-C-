using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Lights.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;

namespace RayTracing.Lights
{
    class AmbientOccluder : Light
    {
        private Vec3 u, v, w;
        private Sampler sampler;
        private double minAmount;
        private Vec3 color;
        private double ls;

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                if(sampler != null)
                    sampler.MapSamplesToHemisphere(1);
            }
        }

        public double ScaleRadiance
        {
            get { return ls; }
            set
            {
                ls = value;
            }
        }

        public double MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
            }
        }

        public Vec3 Color
        {
            get { return color; }
            set
            {
                color = value;
            }
        }
        
        public AmbientOccluder() :
            base()
        {
            minAmount = 0.25;
            color = new Vec3(1.0);
            ls = 1.0;
        }

        public AmbientOccluder(Vec3 color) :
            this(color, 1.0)
        {}

        public AmbientOccluder(Vec3 color, double scaleRadiance) :
            this(color, scaleRadiance, 0.25)
        {}

        public AmbientOccluder(Vec3 color, double scaleRadiance, double minAmount) :
            this(color, scaleRadiance, minAmount, null)
        {}

        public AmbientOccluder(Vec3 color, double scaleRadiance, double minAmount, Sampler sampler) :
            base()
        {
            this.color = color;
            ls = scaleRadiance;
            this.minAmount = minAmount;
            Sampler = sampler;
        }

        public AmbientOccluder(AmbientOccluder other) : 
            base(other)
        {
            u = other.u.Clone();
            v = other.v.Clone();
            w = other.w.Clone();
            minAmount = other.minAmount;
            color = other.color.Clone();
            ls = other.ls;

            if (other.sampler != null)
                sampler = other.sampler;
        }

        public void SetColor(float rgb)
        {
            color.Set(rgb);
        }

        public void SetColor(float r, float g, float b)
        {
            color.Set(r, g, b);
        }

        public void SetSampler(Sampler sampler, double e)
        {
            this.sampler = sampler;
            this.sampler.MapSamplesToHemisphere(e);
        }

        public override Light Clone()
        {
            return new AmbientOccluder(this);
        }

        public override Vec3 GetDirection(ShadeRec sr)
        {
            Vec3 sp = sampler.SampleHemisphere();
            return (sp.X * u + sp.Y * v + sp.Z * w);
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            return sr.World.ShadowHitObjects(ray, MathUtils.HugeValue);
        }

        public override Vec3 L(ShadeRec sr)
        {
            w = sr.Normal;
            v = Vec3.Cross(w, new Vec3(0.0072, 1.0, 0.0034));
            v.Normalize();
            u = Vec3.Cross(v, w);

            Ray shadowRay = new Ray(sr.HitPoint, GetDirection(sr));

            if (InShadow(shadowRay, sr))
                return (minAmount * ls * color);

            return ls * color;
        }
    }
}
