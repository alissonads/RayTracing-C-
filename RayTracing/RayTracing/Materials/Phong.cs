using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.BRDFs;
using RayTracing.Samplers;
using RayTracing.Samplers.Base;

namespace RayTracing.Materials
{
    class Phong : AbsMaterial
    {
        private Lambertian diffuse;
        private GlossySpecular specular;

        public Lambertian Diffuse
        {
            get { return diffuse; }
            set
            {
                diffuse = value;
            }
        }

        public GlossySpecular Specular
        {
            get { return specular; }
            set
            {
                specular = value;
            }
        }

        public Phong() :
            base()
        {
            diffuse = new Lambertian();
            specular = new GlossySpecular();
        }

        public Phong(Vec3 color, float ka, float kd, float ks, float exp) :
            base(color, ka)
        {
            diffuse = new Lambertian(kd, color);
            specular = new GlossySpecular(ks, color, exp);
        }

        public Phong(Phong other) :
            base(other)
        {
            diffuse = (Lambertian)other.diffuse.Clone();
            specular = (GlossySpecular)other.specular.Clone();
        }

        public void SetKd(float kd)
        {
            diffuse.Kd = kd; 
        }

        public void SetKs(float ks)
        {
            specular.Ks = ks;
        }

        public void SetExp(float exp)
        {
            specular.Exp = exp;
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

        public void SetSpecularColor(Vec3 color)
        {
            specular.Cs = color;
        }


        public void SetSpecularColor(float r, float g, float b)
        {
            specular.SetCs(r, g, b);
        }

        public void SetSpecularColor(float rgb)
        {
            specular.SetCs(rgb);
        }

        public virtual void SetColor(Vec3 color)
        {
            SetAmbientColor(color);
            SetDiffuseColor(color);
            SetSpecularColor(color);
        }

        public virtual void SetColor(float r, float g, float b)
        {
            SetAmbientColor(r, g, b);
            SetDiffuseColor(r, g, b);
            SetSpecularColor(r, g, b);
        }

        public virtual void SetColor(float rgb)
        {
            SetAmbientColor(rgb);
            SetDiffuseColor(rgb);
            SetSpecularColor(rgb);
        }

        public void SetCd(Vec3 color)
        {
            SetAmbientColor(color);
            SetDiffuseColor(color);
        }

        public void SetCd(float r, float g, float b)
        {
            SetAmbientColor(r, g, b);
            SetDiffuseColor(r, g, b);
        }

        public void SetCd(float rgb)
        {
            SetAmbientColor(rgb);
            SetDiffuseColor(rgb);
        }

        public void SetSamples(int numSamples)
        {
            diffuse.Sampler = new MultiJittered(numSamples);
            specular.Sampler = new MultiJittered(numSamples);
        }

        public void SetDiffSampler(Sampler sampler)
        {
            diffuse.Sampler = sampler;
        }

        public void SetSpecSampler(Sampler sampler)
        {
            specular.Sampler = sampler;
        }

        public override IMaterial Clone()
        {
            return new Phong(this);
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = ambient.RHO(sr, wo) * sr.World.AmbientLight.L(sr);

            foreach (var light in sr.World.Lights)
            {
                Vec3 wi = light.GetDirection(sr);
                float ndotwi = (float)sr.Normal.Dot(wi);

                if (ndotwi > 0.0)
                {
                    bool inShadow = false;

                    if (light.Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        inShadow = light.InShadow(shadowRay, sr);
                    }

                    if (!inShadow)
                        L +=  (Diffuse.F(sr, wi, wo) + 
                               specular.F(sr, wi, wo)) * 
                               light.L(sr) * ndotwi;
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
                        L += (Diffuse.F(sr, wi, wo) +
                              specular.F(sr, wi, wo)) *
                             light.L(sr) *
                             light.G(sr) * ndotwi / 
                             light.PDF(sr);
                }
            }

            return L;
        }

        public override Vec3 PathShade(ShadeRec sr)
        {
            Vec3 L = new Vec3();
            Vec3 wo = sr.Ray.D * -1;
            Vec3 wiDiff = null;
            Vec3 wiSpec = null;
            float pdfDiff = 0.0f;
            float pdfSpec = 0.0f;

            Vec3 Ldiff = diffuse.SampleF(sr, ref wiDiff, wo, ref pdfDiff);
            Vec3 Lspec = specular.SampleF(sr, ref wiSpec, wo, ref pdfSpec);
            double ndotwiDiff = sr.Normal.Dot(wiDiff);
            double ndotwiSpec = sr.Normal.Dot(wiSpec);

            Ray diffRay = new Ray(sr.HitPoint, wiDiff);
            Ray specRay = new Ray(sr.HitPoint, wiSpec);

            Ldiff *= sr.World.Tracer.TraceRay(diffRay, sr.Depth + 1)
                          * ndotwiDiff / pdfDiff;

            Lspec *= sr.World.Tracer.TraceRay(specRay, sr.Depth + 1)
                          * ndotwiSpec / pdfSpec;

            L += Ldiff + Lspec;
            
            return L;
        }

        public override Vec3 GlobalShade(ShadeRec sr)
        {
            Vec3 wo = sr.Ray.D * -1;
            Vec3 L = new Vec3();

            if (sr.Depth == 0)
                L = AreaLightShade(sr);
            
            Vec3 wiDiff = null;
            Vec3 wiSpec = null;
            float pdfDiff = 0.0f;
            float pdfSpec = 0.0f;

            Vec3 Ldiff = diffuse.SampleF(sr, ref wiDiff, wo, ref pdfDiff);
            Vec3 Lspec = specular.SampleF(sr, ref wiSpec, wo, ref pdfSpec);
            double ndotwiDiff = sr.Normal.Dot(wiDiff);
            double ndotwiSpec = sr.Normal.Dot(wiSpec);

            Ray diffRay = new Ray(sr.HitPoint, wiDiff);
            Ray specRay = new Ray(sr.HitPoint, wiSpec);

            Ldiff *= sr.World.Tracer.TraceRay(diffRay, sr.Depth + 1)
                          * ndotwiDiff / pdfDiff;

            Lspec *= sr.World.Tracer.TraceRay(specRay, sr.Depth + 1)
                          * ndotwiSpec / pdfSpec;

            L += Ldiff + Lspec;

            return L;
        }
    }
}
