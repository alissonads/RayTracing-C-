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
    class PlaneChecker : Base.ITexture
    {
        private float size;
        private float outlineWidth;
        private Vec3 color1;
        private Vec3 color2;
        private Vec3 outlineColor;

        public float Size
        {
            get { return size; }
            set
            {
                size = value;
            }
        }

        public float OutlineWidth
        {
            get { return outlineWidth; }
            set
            {
                outlineWidth = value;
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

        public Vec3 OutlineColor
        {
            get { return outlineColor; }
            set
            {
                outlineColor = value;
            }
        }

        public PlaneChecker()
        {
            size = 1;
            outlineWidth = 0;
            color1 = new Vec3();
            Color2 = new Vec3(1);
            outlineColor = new Vec3(0.1, 0.1, 0.5);
        }

        public PlaneChecker(PlaneChecker other)
        {
            size = other.size;
            outlineWidth = other.outlineWidth;
            color1 = other.color1.Clone();
            color2 = other.color2.Clone();
            outlineColor = other.outlineColor.Clone();
        }

        public ITexture Clone()
        {
            throw new NotImplementedException();
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

        public void SetOutlineColor(float r, float g, float b)
        {
            outlineColor.Set(r, g, b);
        }

        public void SetOutlineColor(float rgb)
        {
            outlineColor.Set(rgb);
        }

        public Vec3 GetColor(ShadeRec sr)
        {
            double x = sr.LocalHitPoint.X;
            double z = sr.LocalHitPoint.Z;
            int ix = (int)Math.Floor(x / size);
            int iz = (int)Math.Floor(z / size);
            double fx = x / size - ix;
            double fz = z / size - iz;
            double width = 0.5 * outlineWidth / size;
            bool inOutline = (fx < width || fx > 1.0 - width) || 
                             (fz < width || fz > 1.0 - width);
            

            if((ix + iz) % 2 == 0)
            {
                if (!inOutline)
                    return color1;
            }
            else
            {
                if (!inOutline)
                    return color2;
            }

            return outlineColor;
        }
    }
}
