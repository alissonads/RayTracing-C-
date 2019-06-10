using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util.Math
{
    sealed class Vec2
    {
        private double x;
        private double y;

        public Vec2()
        {
            x = 0;
            y = 0;
        }

        public Vec2(double xy)
        {
            x = xy;
            y = xy;
        }

        public Vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2(Vec2 other)
        {
            x = other.x;
            y = other.y;
        }

        public Vec2 Clone()
        {
            return new Vec2(this);
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vec2 Set(double x, double y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        public Vec2 Set(double v)
        {
            this.x = v;
            this.y = v;
            return this;
        }

        public static Vec2 operator+(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vec2 operator+(Vec2 v, double s)
        {
            return new Vec2(v.x + s, v.y + s);
        }

        public static Vec2 operator+(double s, Vec2 v)
        {
            return new Vec2(s + v.x, s + v.y);
        }

        public static Vec2 operator-(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vec2 operator-(Vec2 v, double s)
        {
            return new Vec2(v.x - s, v.y - s);
        }

        public static Vec2 operator-(double s, Vec2 v)
        {
            return new Vec2(s - v.x, s - v.y);
        }

        public static Vec2 operator*(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vec2 operator*(Vec2 v, double s)
        {
            return new Vec2(v.x * s, v.y * s);
        }

        public static Vec2 operator*(double s, Vec2 v)
        {
            return new Vec2(s * v.x, s * v.y);
        }

        public static Vec2 operator/(Vec2 v, double s)
        {
            return new Vec2(v.x / s, v.y / s);
        }

        public Vec2 Neg()
        {
            x *= -1;
            y *= -1;
            return this;
        }

        public double SizeSQR
        {
            get { return (x * x) + (y * y); }
        }

        public double Size
        {
            get { return System.Math.Sqrt(SizeSQR); }
        }

        public Vec2 Normalize()
        {
            double s = 1.0 / Size;
            x *= s;
            y *= s;

            return this;
        }

        public Vec2 Rotate(double radius)
        {
            double sin = System.Math.Sin(radius);
            double cos = System.Math.Cos(radius);

            double _x = x * cos - y * sin;
            double _y = x * sin + y * cos;

            return Set(_x, _y);
        }

        public Vec2 Refract(Vec2 normal, float index)
        {
            double dot = normal.Dot(normal);
            float k = 1.0f - (float)System.Math.Pow(index, 2) *
                      (1.0f * (float)System.Math.Pow(dot, 2));

            if (k > 0.0f)
            {
                float r = (float)System.Math.Sqrt(k);
                x = index * x - normal.x * (index * dot + r);
                y = index * y - normal.y * (index * dot + r);
            }

            return this;
        }

        public Vec2 Reflect(Vec2 normal)
        {
            double dot = normal.Dot(normal);

            return Set(x - normal.x * 2.0 * dot,
                       y - normal.y * 2.0 * dot);
        }

        public Vec2 Lerp(Vec2 end, double t)
        {
            return Set(x + t * (end.x - x),
                       y + t * (end.y - y));
        }

        public double Angle
        {
            get { return System.Math.Atan2(y, x); }
        }

        public double Dot(Vec2 other)
        {
            return (x * other.x) +
                   (y * other.y);
        }

        public override string ToString()
        {
            return "( " + x + " - " + y + " - " + " )";
        }
    }
}
