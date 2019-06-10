using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Images
{
    class ImageTexture : ITexture
    {
        private int hres;
        private int vres;
        private Image image;
        private IMapping mapping;

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                hres = image.Hres;
                vres = image.Vres;
            }
        }

        public ImageTexture()
        {
            hres = vres = 100;
        }

        public ImageTexture(Image image) :
            this(image, null)
        {}

        public ImageTexture(Image image, IMapping mapping)
        {
            hres = image.Hres;
            vres = image.Vres;
            this.image = image;
            this.mapping = mapping;
        }

        public ImageTexture(ImageTexture other)
        {
            hres = other.hres;
            vres = other.vres;

            if(other.image != null)
                image = other.image.Clone();
            if(other.mapping != null)
                mapping = other.mapping.Clone();
        }

        public ITexture Clone()
        {
            return new ImageTexture(this);
        }

        public void SetMapping(IMapping m)
        {
            mapping = m;
        }

        public Vec3 GetColor(ShadeRec sr)
        {
            int row = 0, column = 0;

            if(mapping != null)
            {
                mapping.GetTexelCoordinates(sr.LocalHitPoint, 
                                           hres, vres, 
                                           ref row, ref column);
            }
            else
            {
                row = (int)(sr.V * (vres - 1));
                column = (int)(sr.U * (hres - 1));
            }

            return image.GetColor(row, column);
        }
    }
}
