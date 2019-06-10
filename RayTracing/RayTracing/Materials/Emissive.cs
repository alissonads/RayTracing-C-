using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Materials
{
    class Emissive : AbsMaterial, IEmissiveMaterial
    {
        private float ls;
        private Vec3 ce;

        public float ScaleRadiance
        {
            get { return ls; }
            set
            {
                ls = value;
            }
        }

        public Vec3 Ce
        {
            get { return ce; }
            set
            {
                ce = value;
            }
        }

        public Emissive() :
            base()
        {}

        public Emissive(Vec3 ce) :
            this(ce, 1.0f)
        {
            this.ce = ce;
        }

        public Emissive(Vec3 ce, float ls) :
            base()
        {
            this.ce = ce;
            this.ls = ls;
        }

        public Emissive(Emissive other) :
            base(other)
        {
            ls = other.ls;
            ce = other.ce.Clone();
        }

        public void SetCe(float r, float g, float b)
        {
            ce.Set(r, g, b);
        }

        public override IMaterial Clone()
        {
            return new Emissive(this);
        }

        IEmissiveMaterial IEmissiveMaterial.Clone()
        {
            return new Emissive(this);
        }

        public Vec3 GetLe(ShadeRec sr)
        {
            return ((sr.Normal * -1).Dot(sr.Ray.D) > 0) ?
                    ce * ls : ColorUtils.BLACK;
        }

        public override Vec3 Shade(ShadeRec sr)
        {
            return GetLe(sr);
        }

        public override Vec3 AreaLightShade(ShadeRec sr)
        {
            return GetLe(sr);
        }

        public override Vec3 PathShade(ShadeRec sr)
        {
            return GetLe(sr);
        }

        public override Vec3 GlobalShade(ShadeRec sr)
        {
            return (sr.Depth == 1)? ColorUtils.BLACK
                                  : GetLe(sr);
        }

    }
}
