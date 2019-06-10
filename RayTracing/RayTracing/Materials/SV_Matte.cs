using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.BRDFs;
using RayTracing.Textures.Base;

namespace RayTracing.Materials
{
    class SV_Matte : IMaterial
    {
        private SV_Lambertian ambient;
        private SV_Lambertian diffuse;

        public SV_Matte() :
             this(new SV_Lambertian(),
                  new SV_Lambertian())
        {}

        public SV_Matte(SV_Lambertian ambient,
                        SV_Lambertian diffuse)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
        }

        public SV_Matte(ITexture cd, float ka, float kd)
        {
            ambient = new SV_Lambertian(ka, cd);
            diffuse = new SV_Lambertian(kd, cd);
        }

        public SV_Matte(SV_Matte other)
        {
            ambient = (SV_Lambertian)other.ambient.Clone();
            diffuse = (SV_Lambertian)other.diffuse.Clone();
        }

        public  IMaterial Clone()
        {
            return new SV_Matte(this);
        }

        public void SetCd(ITexture t)
        {
            ambient.Cd = t;
            diffuse.Cd = t;
        }

        public void SetKa(float ka)
        {
            ambient.Kd = ka;
        }

        public void SetKd(float kd)
        {
            diffuse.Kd = kd;
        }

        public Vec3 Shade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = ambient.RHO(sr, wo) * 
                     sr.World.AmbientLight.L(sr);

            foreach (var light in sr.World.Lights)
            {
                Vec3 wi = light.GetDirection(sr);
                wi.Normalize();
                float ndotwi = (float)sr.Normal.Dot(wi);
                float ndotwo = (float)sr.Normal.Dot(wo);

                if(ndotwi > 0 && ndotwo > 0)
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
                            ndotwi; ;
                }
            }

            return L;
        }
        
        public Vec3 AreaLightShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = ambient.RHO(sr, wo) *
                     sr.World.AmbientLight.L(sr);

            foreach (var light in sr.World.Lights)
            {
                Vec3 wi = light.GetDirection(sr);
                wi.Normalize();
                float ndotwi = (float)sr.Normal.Dot(wi);
                float ndotwo = (float)sr.Normal.Dot(wo);

                if (ndotwi > 0 && ndotwo > 0)
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
                             ndotwi / light.PDF(sr) ;
                }
            }

            return L;
        }
        
        public Vec3 PathShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wi = null;
            float pdf = 0.0f;
            Vec3 f = diffuse.SampleF(sr, ref wi, wo, ref pdf);
            Ray reflectedRay = new Ray(sr.HitPoint, wi);

            return (f * sr.World.Tracer.TraceRay(reflectedRay, sr.Depth + 1)
                      * sr.Normal.Dot(wi) / pdf);
        }

        public Vec3 GlobalShade(ShadeRec sr)
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
