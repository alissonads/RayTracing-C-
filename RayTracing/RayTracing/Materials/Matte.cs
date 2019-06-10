using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Materials.Base;
using RayTracing.BRDFs;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.Samplers.Base;

namespace RayTracing.Materials
{
    class Matte : AbsMaterial
    {
        private Lambertian diffuse;

        public Lambertian Diffuse
        {
            get { return diffuse; }
            set
            {
                diffuse = value;
            }
        }

        public Matte() :
            base()
        {
            diffuse = new Lambertian();
        }

        public Matte(Vec3 color, float ka, float kd) :
            base(color, ka)
        {
            diffuse = new Lambertian(kd, color);
        }

        public Matte(Matte other) :
            base(other)
        {
            diffuse = (Lambertian)other.diffuse.Clone();
        }

        public void SetKd(float kd)
        {
            diffuse.Kd = kd;
        }

        public void SetDiffuseColor(Vec3 color)
        {
            diffuse.Cd = color;
        }

        public void SetDiffuseColor(float r, float g, float b)
        {
            diffuse.SetCd(r, g, b);
        }

        public void SetDiffuseColor(float rgb)
        {
            diffuse.SetCd(rgb);
        }

        public virtual void SetColor(Vec3 color)
        {
            SetAmbientColor(color);
            SetDiffuseColor(color);
        }

        public virtual void SetColor(float r, float g, float b)
        {
            SetAmbientColor(r, g, b);
            SetDiffuseColor(r, g, b);
        }

        public virtual void SetColor(float rgb)
        {
            SetAmbientColor(rgb);
            SetDiffuseColor(rgb);
        }

        public void SetSampler(Sampler sampler)
        {
            diffuse.Sampler = sampler;
        }

        public override IMaterial Clone()
        {
            return new Matte(this);
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = ambient.RHO(sr, wo) * sr.World.AmbientLight.L(sr);

            foreach (var light in sr.World.Lights)
            {
                Vec3 wi = light.GetDirection(sr);
                float ndotwi = (float)sr.Normal.Dot(wi);

                if(ndotwi > 0.0f)
                {
                    bool inShadow = false;

                    if(light.Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        inShadow = light.InShadow(shadowRay, sr);
                    }

                    if (!inShadow)
                        L += diffuse.F(sr, wi, wo) * 
                             light.L(sr) * 
                             ndotwi;
                }
            }

            return L;
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = ambient.RHO(sr, wo) * sr.World.AmbientLight.L(sr);

            foreach (var light in sr.World.Lights)
            {
                Vec3 wi = light.GetDirection(sr);
                float ndotwi = (float)sr.Normal.Dot(wi);

                if (ndotwi > 0.0f)
                {
                    bool inShadow = false;

                    if (light.Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        inShadow = light.InShadow(shadowRay, sr);
                    }

                    if (!inShadow)
                        L += diffuse.F(sr, wi, wo) * 
                             light.L(sr) * 
                             light.G(sr) *
                             ndotwi / light.PDF(sr);
                }
            }

            return L;
        }

        public override Vec3 PathShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0.0f;
            Vec3 f = diffuse.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            return (f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                      * sr.Normal.Dot(wi) / pdf);
        }

        public override Vec3 GlobalShade(ShadeRec sr)
        {
            Vec3 L = new Vec3();

            if (sr.Depth == 0)
                L = AreaLightShade(sr);

            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0;
            Vec3 f = diffuse.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);
            L += f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                   * sr.Normal.Dot(wi) / pdf;

            return L;
        }
    }
}
