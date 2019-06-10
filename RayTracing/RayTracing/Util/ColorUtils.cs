using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util
{
    using Math;

    class ColorUtils
    {
        public static Vec3 BLACK  = new Vec3(0.0, 0.0, 0.0);
        public static Vec3 WHITE  = new Vec3(1.0, 1.0, 1.0);
        public static Vec3 RED    = new Vec3(1.0, 0.0, 0.0);
        public static Vec3 GREEN  = new Vec3(0.0, 1.0, 0.0);
        public static Vec3 BLUE   = new Vec3(0.0, 0.0, 1.0);
        public static Vec3 YELLOW = new Vec3(1.0, 1.0, 0.0);
        public static int MAX_COLOR = 0xFF;

        public static Vec3 Powc(Vec3 color, double d)
        {
            return new Vec3(System.Math.Pow(color.X, d),
                            System.Math.Pow(color.Y, d),
                            System.Math.Pow(color.Z, d));
        }

        public static Vec3 Saturate(Vec3 color)
        {
            Vec3 finalColor = new Vec3();
            finalColor.X = color.X > 1.0 ? 1.0 : color.X < 0.0 ? 0.0 : color.X;
            finalColor.Y = color.Y > 1.0 ? 1.0 : color.Y < 0.0 ? 0.0 : color.Y;
            finalColor.Z = color.Z > 1.0 ? 1.0 : color.Z < 0.0 ? 0.0 : color.Z;

            return finalColor;
        }

        public static int ToRGB(Vec3 color)
        {
            int r = (int)(color.X * 0xFF);
            int g = (int)(color.Y * 0xFF);
            int b = (int)(color.Z * 0xFF);

            int rgb =  0xFF      << 24 |
                      (r & 0xFF) << 16 |
                      (g & 0xFF) << 8  |
                      (b & 0xFF) << 0;
            return rgb;
        }
        
    }
}
