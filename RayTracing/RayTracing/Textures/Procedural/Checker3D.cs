using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Procedural
{
    class Checker3D : ITexture
    {
        private float size;
        private Vec3 color1;
        private Vec3 color2;

        public float Size
        {
            get { return size; }
            set
            {
                size = value;
            }
        }

        public Vec3 Color1
        {
            get { return color1; }
            set
            {
                color1 = value;
            }
        }

        public Vec3 Color2
        {
            get { return color2; }
            set
            {
                color2 = value;
            }
        }

        public Checker3D()
        {
            size = 1;
            color1 = new Vec3();
            color2 = new Vec3(1);
        }

        public Checker3D(Checker3D other)
        {
            size = other.size;
            color1 = other.color1.Clone();
            color2 = other.color2.Clone();
        }

        public ITexture Clone()
        {
            return new Checker3D(this);
        }

        public void SetColor1(float r, float g, float b)
        {
            color1.Set(r, g, b);
        }

        public void SetColor1(float rgb)
        {
            color1.Set(rgb);
        }

        public void SetColor2(float r, float g, float b)
        {
            color2.Set(r, g, b);
        }

        public void SetColor2(float rgb)
        {
            color2.Set(rgb);
        }

        public Vec3 GetColor(ShadeRec sr)
        {
            double eps = -0.000187453738;
            double x = sr.LocalHitPoint.X + eps;
            double y = sr.LocalHitPoint.Y + eps;
            double z = sr.LocalHitPoint.Z + eps;

            if (((int)Math.Floor(x / size) + 
                 (int)Math.Floor(y / size)  + 
                 (int)Math.Floor(z / size)) % 2 == 0)
                return color1;

            return color2;
        }
    }
}
