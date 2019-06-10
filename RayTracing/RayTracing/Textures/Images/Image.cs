using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RayTracing.Util.Math;
using RayTracing.Util;
using System.IO;
using RayTracing.Util.PPMFile;

namespace RayTracing.Textures.Images
{
    class Image
    {
        private int hres;
        private int vres;
        private List<Vec3> pixels;

        public int Hres { get { return hres; } }

        public int Vres { get { return vres; } }

        public Image()
        {
            hres = vres = 100;
            pixels = new List<Vec3>();
        }

        public Image(Image other)
        {
            hres = other.hres;
            vres = other.vres;
            pixels = other.pixels.ToList();
        }

        public Image Clone()
        {
            return new Image(this);
        }

        public Vec3 GetColor(int row, int column)
        {
            int index = column + hres * (vres - row - 1);
            int n = pixels.Count;

            if (index < n && index >= 0)
                return pixels[index];

            return ColorUtils.RED;
        }

        public void Load(string path)
        {
            Load(path, IsPPM(path));
        }

        public void Load(string path, bool isPPM)
        {
            if(isPPM)
            {
                PPM ppm = PPM.LoadImage(path);
                LoadFromPPM(ppm);
            }
            else
            {
                Bitmap bitMap = new Bitmap(path);
                LoadFromBitmap(bitMap);
            }
        }

        private void LoadFromBitmap(Bitmap bitMap)
        {
            hres = bitMap.Width;
            vres = bitMap.Height;

            for(int y = 0; y < vres; y++)
                for(int x = 0; x < hres; x++)
                {
                    Color c = bitMap.GetPixel(x, y);
                    pixels.Add(new Vec3(c.R / 255.0,
                                        c.G / 255.0,
                                        c.B / 255.0 ));
                }
        }

        private void LoadFromPPM(PPM ppm)
        {
            hres = ppm.Width;
            vres = ppm.Height;

            for (int y = 0; y < vres; y++)
                for (int x = 0; x < hres; x++)
                {
                    Vec3 c = ppm.GetColorv(x, y);
                    pixels.Add(c);
                }
        }
        
        private bool IsPPM(string path)
        {
            int i = path.LastIndexOf('.');
            string extension = path.Substring(i);

            if (extension == ".ppm")
                return true;

            return false;
        }
    }



}
