using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Lights.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;
using RayTracing.Materials.Base;

namespace RayTracing.Lights
{
    class EnvironmentLight : Light
    {
        private Sampler sampler;
        private IEmissiveMaterial material;
        private Vec3 u, v, w;
        private Vec3 wi;

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                sampler.MapSamplesToHemisphere(1);
            }
        }

        public IEmissiveMaterial Material
        {
            get { return material; }
            set
            {
                material = value;
            }
        }

        public EnvironmentLight() :
            base()
        {}

        public EnvironmentLight(IEmissiveMaterial material, Sampler sampler) :
            base()
        {
            this.material = material;
            this.sampler = sampler;
        }

        public EnvironmentLight(EnvironmentLight other) :
            base(other)
        {
            sampler = other.sampler.Clone();
            material = other.material.Clone();
            u = other.u.Clone();
            v = other.v.Clone();
            w = other.w.Clone();
            wi = other.wi.Clone();
        }

        public void SetSampler(Sampler sampler, double e)
        {
            this.sampler = sampler;
            this.sampler.MapSamplesToHemisphere(e);
        }

        public override Light Clone()
        {
            return new EnvironmentLight(this);
        }
        
        public override Vec3 GetDirection(ShadeRec sr)
        {
            w = sr.Normal;
            v = new Vec3(0.0034, 1.0, 0.0071).Cross(w);
            v.Normalize();
            u = Vec3.Cross(v, w);
            Vec3 sp = sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;

            return wi;
        }

        public override Vec3 L(ShadeRec sr)
        {
            return material.GetLe(sr);
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            return sr.World.ShadowHitObjects(ray, MathUtils.HugeValue);
        }

        public override float PDF(ShadeRec sr)
        {
            return (float)(sr.Normal.Dot(wi) * MathUtils.InvPI);
        }
    }
}
