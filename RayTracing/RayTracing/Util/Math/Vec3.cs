using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util.Math
{
    sealed class Vec3
    {
        private double x;
        private double y;
        private double z;

        public Vec3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vec3(double xyz)
        {
            x = xyz;
            y = xyz;
            z = xyz;
        }

        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public Vec3 Clone()
        {
            return new Vec3(this);
        }

        public double X
        {
            set { x = value; }
            get { return x; }
        }

        public double Y
        {
            set { y = value; }
            get { return y; }
        }

        public double Z
        {
            set { z = value; }
            get { return z; }
        }

        public Vec3 Set(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public Vec3 Set(double v)
        {
            x = v;
            y = v;
            z = v;

            return this;
        }
        
        public Vec3 Set(Vec3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;

            return this;
        }

        public static Vec3 operator+(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x,
                            v1.y + v2.y,
                            v1.z + v2.z);
        }

        public static Vec3 operator+(Vec3 v, double s)
        {
            return new Vec3(v.x + s,
                            v.y + s,
                            v.z + s);
        }

        public static Vec3 operator+(double s, Vec3 v1)
        {
            return new Vec3(s + v1.x,
                            s + v1.y,
                            s + v1.z);
        }

        public static Vec3 operator-(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x,
                            v1.y - v2.y,
                            v1.z - v2.z);
        }

        public static Vec3 operator-(Vec3 v, double s)
        {
            return new Vec3(v.x - s,
                             v.y - s,
                             v.z - s);
        }

        public static Vec3 operator-(double s, Vec3 v)
        {
            return new Vec3(s - v.x,
                             s - v.y,
                             s - v.z);
        }

        public static Vec3 operator*(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x * v2.x,
                            v1.y * v2.y,
                            v1.z * v2.z);
        }

        public static Vec3 operator*(Vec3 v, double s)
        {
            return new Vec3(v.x * s,
                            v.y * s,
                            v.z * s);
        }

        public static Vec3 operator*(double s, Vec3 v)
        {
            return new Vec3(v.x * s,
                            v.y * s,
                            v.z * s);
        }

        public static Vec3 operator/(Vec3 v, double s)
        {
            return new Vec3(v.x / s,
                            v.y / s,
                            v.z / s);
        }

        public Vec3 Neg()
        {
            x *= -1;
            y *= -1;
            z *= -1;
            return this;
        }

        public double SizeSQR
        {
            get { return (x * x) +
                         (y * y) +
                         (z * z);
                }
        }

        public double Size
        {
            get { return System.Math.Sqrt(SizeSQR); }
        }

        public Vec3 Normalize()
        {
            double s = 1.0 / Size;

            return s == 0 ? this :
                   Set(x * s, y * s, z * s);
        }

        public Vec3 RotateX(double radius)
        {
            double sin = System.Math.Sin(radius);
            double cos = System.Math.Cos(radius);

            double _y = (y * cos) - (z * sin);
            double _z = (z * cos) + (y * sin);

            return Set(x, _y, _z);
        }

        public Vec3 RotateY(double radius)
        {
            double sin = System.Math.Sin(radius);
            double cos = System.Math.Cos(radius);

            double _x = (x * cos) + (z * sin);
            double _z = (z * cos) - (x * sin);

            return Set(_x, y, _z);
        }

        public Vec3 RotateZ(double radius)
        {
            double sin = System.Math.Sin(radius);
            double cos = System.Math.Cos(radius);

            double _x = (x * cos) - (y * sin);
            double _y = (y * cos) + (x * sin);

            return Set(_x, _y, z);
        }

        public Vec3 Rotate(Vec3 axis, double radians)
        {
            double sin = System.Math.Sin(radians);
            double cos = System.Math.Cos(radians);
            double k = 1.0 - cos;

            x = x * (cos + k * axis.x * axis.x) +
                y * (k * axis.x * axis.y - sin * axis.z) +
                z * (k * axis.x * axis.z + sin * axis.x);

            y = x * (k * axis.x * axis.y + sin * axis.z) +
                y * (cos + k * axis.y * axis.y) +
                z * (k * axis.y * axis.z - sin * axis.x);

            z = x * (k * axis.x * axis.z - sin * axis.y) +
                y * (k * axis.y * axis.z + sin * axis.x) +
                z * (cos + k * axis.z * axis.z);

            return this;
        }
        
        public static Vec3 Rotate(Vec3 v, Vec3 axis, double radians)
        {
            return v.Clone().Rotate(axis, radians);
        }

        public Vec3 Refract(Vec3 normal, float index)
        {
            double dot = normal.Dot(normal);
            float k = 1.0f - (float)System.Math.Pow(index, 2) *
                      (1.0f * (float)System.Math.Pow(dot, 2));

            if (k >= 0.0f)
            {
                float r = (float)System.Math.Sqrt(k);
                x = index * x - normal.x * (index * dot + r);
                y = index * y - normal.y * (index * dot + r);
                z = index * z - normal.z * (index * dot + r);
            }

            return this;
        }

        public Vec3 Reflect(Vec3 normal)
        {
            double dot = normal.Dot(normal);

            return Set(x - 2.0 * normal.x * dot,
                       y - 2.0 * normal.y * dot,
                       z - 2.0 * normal.z * dot);
        }

        public Vec3 Lerp(Vec3 end, double t)
        {
            return Set(x + t * (end.x - x),
                       y + t * (end.y - y),
                       z + t * (end.z - z));
        }

        public double Dot(Vec3 other)
        {
            return (x * other.x) +
                   (y * other.y) +
                   (z * other.z);
        }

        public Vec3 Cross(Vec3 other)
        {
            return Set((y * other.z) - (z * other.y),
                       (z * other.x) - (x * other.z),
                       (x * other.y) - (y * other.x));
        }

        public static Vec3 Cross(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1).Cross(v2);
        }

        public Vec3 Mix(Vec3 other, float percentage)
        {
            x = x * (1.0f - percentage) + other.x * percentage;
            y = y * (1.0f - percentage) + other.y * percentage;
            z = z * (1.0f - percentage) + other.z * percentage;

            return this;
        }

        public static Vec3 CartesianCoordinates(double ray, double angleZenite, double angleAzimute)
        {
            double x = ray * System.Math.Cos(angleZenite) * System.Math.Cos(angleAzimute);
            double y = ray * System.Math.Sin(angleZenite);
            double z = ray * System.Math.Sin(angleAzimute) * System.Math.Cos(angleZenite);

            return new Vec3(x, y, z); 
        }

        public Vec3 PolarCoordinates()
        {
            double ray = Size;
            double zenite = MathUtils.ToDegrees(System.Math.Atan2(z, x));
            double azimute = MathUtils.ToDegrees(System.Math.Atan2(new Vec2(x, y).Size, y));

            return Set(ray, zenite, azimute);
        }

        public override string ToString()
        {
            return "( " + x + " - " + y + " - " + z + " )";
        }

    }
}
