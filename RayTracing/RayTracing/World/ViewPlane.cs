using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;
using RayTracing.Samplers.Base;
using RayTracing.Samplers;
using RayTracing.Util;

namespace RayTracing.World
{
    enum SystemOfCoordinates
    {
        SSC_INT,
        SSC_FLOAT
    }

    class ViewPlane
    {
        private int hres;
        private int vres;
        private float s;
        private float gama;
        private float invGama;
        private int numSamples;
        private Vec2 pixelSize;
        private Sampler sampler;
        private int showOutOfGamut;
        private int maxDepth;

        private int []pixels;
        
        public int Hres
        {
            get { return hres; }
            set { hres = value; }
        }

        public int Vres
        {
            get { return vres; }
            set { vres = value; }
        }

        public float S
        {
            get { return s; }
            set { s = value; }
        }

        public float Gama
        {
            get { return gama; }
            set { gama = value; }
        }

        public float InvGama
        {
            get { return invGama; }
            set { invGama = value; }
        }

        public int NumSamples
        {
            get { return numSamples; }
            set
            {
                numSamples = value;

                if (numSamples > 1)
                    sampler = new MultiJittered(numSamples);
                else
                    sampler = new Regular(1);
            }
        }

        public Vec2 PixelSize
        {
            get { return pixelSize; }
            set { pixelSize = value; }
        }

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                numSamples = value.NumSamples;
                sampler = value;
                sampler.Generate();
            }
        }

        public int ShowOutOfGamut
        {
            get { return showOutOfGamut; }
            set
            {
                showOutOfGamut = value;
            }
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set
            {
                maxDepth = value;
            }
        }

        public int[] Pixels
        {
            get { return pixels; }
        }

        private void CreatePixels(int hres, int vres)
        {
            if(pixels == null)
                pixels = new int[hres * vres];
        }

        private  ViewPlane() :
            this(400, 400, new Vec2(1, 1))
        {}

        private ViewPlane(int hres, int vres, Vec2 pixelSize)
        {
            Hres = hres;
            Vres = vres;
            S = 1.0f;
            Gama = 1.0f;
            InvGama = 1.0f;
            NumSamples = 0;
            PixelSize = pixelSize;
            MaxDepth = 1;
            CreatePixels(hres, vres);
        }

        private ViewPlane(ViewPlane other)
        {
            hres = other.Hres;
            vres = other.Vres;
            s = other.S;
            gama = other.Gama;
            invGama = other.InvGama;
            numSamples = other.NumSamples;
            pixelSize = other.PixelSize.Clone();
            sampler = other.sampler.Clone();
            showOutOfGamut = other.showOutOfGamut;
            maxDepth = other.maxDepth;
            pixels = other.pixels.ToArray();
        }

        public static ViewPlane Create(int hres, int vres, SystemOfCoordinates ssc)
        {
            switch (ssc)
            {
                case SystemOfCoordinates.SSC_INT:
                    return new ViewPlane(hres, vres, new Vec2(1, 1));
                case SystemOfCoordinates.SSC_FLOAT:
                    return new ViewPlane(hres, vres, new Vec2(2.0/hres, 2.0/vres));
            }

            return new ViewPlane();
        }

        public ViewPlane Clone()
        {
            return new ViewPlane(this);
        }

        public void SetPixelColor(int x, int y, Vec3 color)
        {
            int rgb = ColorUtils.ToRGB(ColorUtils.Saturate(color)); 
            int i = (y * hres) + x;
            pixels[i] = rgb;
        }

        public int GetPixelColor(int x, int y)
        {
            int i = (y * hres) + x;
            return pixels[i];
        }
    }
}
