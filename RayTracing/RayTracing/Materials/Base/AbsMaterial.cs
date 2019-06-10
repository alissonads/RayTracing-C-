using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.BRDFs;
using RayTracing.Util.Math;
using RayTracing.Util;

namespace RayTracing.Materials.Base
{
    abstract class AbsMaterial : IMaterial
    {
        protected Lambertian ambient;

        public Lambertian Ambient
        {
            get { return ambient; }
            set
            {
                ambient = value;
            }
        }

        public AbsMaterial()
        {
            ambient = new Lambertian();
        }

        public AbsMaterial(Lambertian ambient)
        {
            this.ambient = ambient;
        }

        public AbsMaterial(Vec3 color, float ka) :
            this(new Lambertian(ka, color))
        {}

        public AbsMaterial(AbsMaterial other)
        {
            ambient = (Lambertian)other.ambient.Clone();
        }

        public void SetKa(float ka)
        {
            ambient.Kd = ka;
        }

        public void SetAmbientColor(Vec3 color)
        {
            ambient.Cd = color;
        }

        public void SetAmbientColor(float r, float g, float b)
        {
            ambient.SetCd(r, g, b);
        }

        public void SetAmbientColor(float rgb)
        {
            ambient.SetCd(rgb);
        }

        public abstract IMaterial Clone();

        public virtual Vec3 Shade(ShadeRec sr)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 AreaLightShade(ShadeRec sr)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 PathShade(ShadeRec sr)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 GlobalShade(ShadeRec sr)
        {
            return ColorUtils.BLACK;
        }
    }
}
