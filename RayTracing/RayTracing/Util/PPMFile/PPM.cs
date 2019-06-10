using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using RayTracing.Util.Math;

namespace RayTracing.Util.PPMFile
{
    enum TypeData
    {
        PPM_ASCII,
        PPM_BINARY,
        PPM_LONGCOLOR_END_BINARY
    }

    class PPM
    {
        private int width;
        private int height;
        private int maxColor = 255;
        private List<int> pixels;
        private TypeData type = TypeData.PPM_BINARY;

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public int []Pixels
        {
            get { return pixels.ToArray(); }
        }

        public int RMask
        {
            get { return 0xFF0000; }
        }

        public int GMask
        {
            get { return 0x00FF00; }
        }

        public int BMask
        {
            get { return 0x0000FF; }
        }

        public int RShift
        {
            get { return 16; }
        }

        public int GShift
        {
            get { return 8; }
        }

        public int BShift
        {
            get { return 0; }
        }

        private PPM()
        {
            width = height = 0;
        }

        private PPM(PPM other)
        {
            width = other.width;
            height = other.height;
            maxColor = other.maxColor;
            pixels = other.pixels.ToList();
            type = other.type;
        }

        public static PPM Create(int width, int height)
        {
            return Create(width, height, 0xFF, TypeData.PPM_BINARY);
        }

        public static PPM Create(int width, int height, int maxColor, TypeData type)
        {
            PPM ppm = new PPM();
            ppm.width = width;
            ppm.height = height;
            ppm.maxColor = maxColor;
            ppm.type = type;
            ppm.CreatePixels();

            return ppm;
        }
        
        public PPM Clone()
        {
            return new PPM(this);
        }

        public void SetColor(int x, int y, int r, int g, int b)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();

            pixels[index] = r;
            pixels[index+1] = g;
            pixels[index+2] = b;
        }

        public void SetColor(int x, int y, int color)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();

            int r = (color & RMask) >> RShift;
            int g = (color & GMask) >> GShift;
            int b = (color & BMask) >> BShift;

            pixels[index] = r;
            pixels[index + 1] = g;
            pixels[index + 2] = b;
        }

        public int[] GetChannels(int x, int y)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();
                

            return new[] { pixels[index],
                           pixels[index+1],
                           pixels[index+2] };
        }

        public double[] GetChannelsf(int x, int y)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();


            return new[] { (double)pixels[index] / maxColor,
                           (double)pixels[index+1] / maxColor,
                           (double)pixels[index+2] / maxColor };
        }

        public int GetColor(int x, int y)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();


            return ((pixels[index]   << RShift) |
                    (pixels[index+1] << GShift) |
                    (pixels[index+2] << BMask )  );
        }

        public Vec3 GetColorv(int x, int y)
        {
            int index = ((y * width) + x) * 3;
            int n = pixels.Count;

            if (index > n || index < 0 || (index + 1) > n)
                throw new IndexOutOfRangeException();
            
            return new Vec3((double)pixels[index]   / maxColor,
                            (double)pixels[index+1] / maxColor,
                            (double)pixels[index+2] / maxColor);
        }

        public static PPM LoadImage(string path)
        {
            return ToCreateThroughAnExistentFile(path);
        }

        private static PPM ToCreateThroughAnExistentFile(string path)
        {
            PPM ppm = new PPM();
            ppm.ReadFile(path);

            return ppm;
        }

        private void ReadFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    using (Stream stream = File.Open(path, FileMode.Open))
                    {
                        ReadFile(stream);
                    }
                }
                catch (Exception e)
                {
                    throw new IOException(e.ToString());
                }
            }
            else
                throw new IOException("file doesn't exist");
        }

        private void ReadFile(Stream stream)
        {
            Setup(stream);
            ReadData(stream);
        }

        private void Setup(Stream reader)
        {
            string r = ReadWorld(reader);

            if (!IsPPM(r))
            {
                throw new IOException("File is not PPM or is Corrupted");
            }

            bool binary = IsBinary(r);
            
            width = int.Parse(ReadWorld(reader));
            height = int.Parse(ReadWorld(reader));

            maxColor = int.Parse(ReadWorld(reader));

            if (maxColor > 0xFF && binary)
                type = TypeData.PPM_LONGCOLOR_END_BINARY;
            else if (binary)
                type = TypeData.PPM_BINARY;
            else
                type = TypeData.PPM_ASCII;

            CreatePixels();
        }

        private void ReadData(Stream reader)
        {
            switch (type)
            {
                case TypeData.PPM_ASCII:
                    ReadAscII(reader);
                    break;
                case TypeData.PPM_BINARY:
                    ReadBinary(reader);
                    break;
                case TypeData.PPM_LONGCOLOR_END_BINARY:
                    ReadByte(reader);
                    break;
            }
        }
        
        private void CreatePixels()
        {
            int size = width * height * 3;

            if (pixels != null)
                pixels.Capacity = size;
            else
                pixels = new List<int>(size);
        }

        private string ReadWorld(Stream reader)
        {
            StringBuilder s = new StringBuilder();

            char c = (char)reader.ReadByte();

            while (c == '#')
            {
                while ("\r\n".IndexOf(c) == -1)
                {
                    c = (char)reader.ReadByte();
                }
                c = (char)reader.ReadByte();
            }

            while (" \t\r\n".IndexOf(c) == -1)
            {
                s.Append(c);
                c = (char)reader.ReadByte();
            }

            if (s.Length == 0)
                return ReadWorld(reader);

            return s.ToString().Trim();
        }

        public string ReadChannel(Stream reader)
        {
            return ReadWorld(reader);
        }

        public int ReadChannelb(Stream reader)
        {
            return reader.ReadByte();
        }

        private bool IsPPM(string s)
        {
            return (s == "P3" || s == "P6");
        }

        private bool IsBinary(string s)
        {
            return (s == "P6");
        }

        private List<int> GetProperty(string s)
        {
            List<int> p = new List<int>();
            string []parts = s.Split(' ');

            foreach (var item in parts)
            {
                p.Add(int.Parse(item));
            }

            return p;
        }
        
        private void ReadByte(Stream reader)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = (ReadChannelb(reader) << 8) | ReadChannelb(reader);
                    int g = (ReadChannelb(reader) << 8) | ReadChannelb(reader);
                    int b = (ReadChannelb(reader) << 8) | ReadChannelb(reader);

                    pixels.Add(r);
                    pixels.Add(g);
                    pixels.Add(b);
                }
            }
        }

        private void ReadBinary(Stream reader)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = ReadChannelb(reader);
                    int g = ReadChannelb(reader);
                    int b = ReadChannelb(reader);

                    pixels.Add(r);
                    pixels.Add(g);
                    pixels.Add(b);
                }
            }
        }

        private void ReadAscII(Stream reader)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    int r = int.Parse(ReadChannel(reader));
                    int g = int.Parse(ReadChannel(reader));
                    int b = int.Parse(ReadChannel(reader));

                    pixels.Add(r);
                    pixels.Add(g);
                    pixels.Add(b);
                }
            }
        }

    }
}
