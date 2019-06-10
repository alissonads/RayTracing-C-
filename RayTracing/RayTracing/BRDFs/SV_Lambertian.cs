using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BRDFs.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Textures.Base;

namespace RayTracing.BRDFs
{
    class SV_Lambertian : BRDF
    {
        private float kd;
        private ITexture cd;

        public float Kd
        {
            get { return kd; }
            set
            {
                kd = value;
            }
        }

        public ITexture Cd
        {
            get { return cd; }
            set
            {
                cd = value;
            }
        }

        public SV_Lambertian() :
            base()
        {
            kd = 0;
        }

        public SV_Lambertian(float kd, ITexture cd) :
            base()
        {
            this.kd = kd;
            this.cd = cd;
        }

        public SV_Lambertian(SV_Lambertian other) :
            base(other)
        {
            kd = other.kd;
            cd = other.cd.Clone();
        }

        public override BRDF Clone()
        {
            return new SV_Lambertian(this);
        }

        public override Vec3 F(ShadeRec sr, Vec3 wi, Vec3 wo)
        {
            return kd * cd.GetColor(sr) * MathUtils.InvPI;
        }

        public override Vec3 RHO(ShadeRec sr, Vec3 wo)
        {
            return kd * cd.GetColor(sr);
        }
    }
}
