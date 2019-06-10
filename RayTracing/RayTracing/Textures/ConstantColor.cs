using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Textures
{
    class ConstantColor : ITexture
    {
        private Vec3 color;
        
        public ConstantColor() :
            this(new Vec3(1.0))
        {}

        public ConstantColor(Vec3 color)
        {
            this.color = color;
        }

        public ConstantColor(ConstantColor other)
        {
            color = other.color.Clone();
        }

        public ITexture Clone()
        {
            return new ConstantColor(this);
        }

        public Vec3 GetColor(ShadeRec sr)
        {
            return color;
        }

        public void SetColor(Vec3 color)
        {
            this.color = color;
        }

        public void SetColor(float r, float g, float b)
        {
            color.Set(r, g, b);
        }

        public void SetColor(float rgb)
        {
            color.Set(rgb);
        }
    }
}
