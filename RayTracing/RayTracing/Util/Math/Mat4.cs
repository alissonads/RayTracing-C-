using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util.Math
{
    sealed class Mat4
    {
        private double[][] m;

        private void Alloc()
        {
            if (m == null)
            {
                m = new double[4][];

                for (int i = 0; i < 4; i++)
                {
                    m[i] = new double[4];
                }
            }
        }

        public Mat4()
        {
            Alloc();
        }

        public Mat4(double m0x0, double m0x1, double m0x2, double m0x3,
                    double m1x0, double m1x1, double m1x2, double m1x3,
                    double m2x0, double m2x1, double m2x2, double m2x3,
                    double m3x0, double m3x1, double m3x2, double m3x3)
        {
            Alloc();
            Set(m0x0, m0x1, m0x2, m0x3,
                m1x0, m1x1, m1x2, m1x3,
                m2x0, m2x1, m2x2, m2x3,
                m3x0, m3x1, m3x2, m3x3);
        }

        public Mat4(double m0x0, double m0x1, double m0x2,
                    double m1x0, double m1x1, double m1x2,
                    double m2x0, double m2x1, double m2x2)
        {
            Alloc();
            Set(m0x0, m0x1, m0x2,
                m1x0, m1x1, m1x2,
                m2x0, m2x1, m2x2);
        }

        public Mat4(Mat4 other)
        {
            Alloc();
            Set(other);
        }

        public Mat4(Vec3 v1, Vec3 v2, Vec3 v3)
        {
            Alloc();
            Set(v1, v2, v3);
        }

        public Mat4 Clone()
        {
            return new Mat4(this);
        }

        public double[][] Matrix
        {
            set { m = (double[][])value.Clone(); }
            get { return m; }
        }

        public Mat4 Set(double m0x0, double m0x1, double m0x2, double m0x3,
                        double m1x0, double m1x1, double m1x2, double m1x3,
                        double m2x0, double m2x1, double m2x2, double m2x3,
                        double m3x0, double m3x1, double m3x2, double m3x3)
        {
            m[0][0] = m0x0; m[0][1] = m0x1; m[0][2] = m0x2; m[0][3] = m0x3;
            m[1][0] = m1x0; m[1][1] = m1x1; m[1][2] = m1x2; m[1][3] = m1x3;
            m[2][0] = m2x0; m[2][1] = m2x1; m[2][2] = m2x2; m[2][3] = m2x3;
            m[3][0] = m3x0; m[3][1] = m3x1; m[3][2] = m3x2; m[3][3] = m3x3;

            return this;
        }

        public Mat4 Set(double m0x0, double m0x1, double m0x2,
                        double m1x0, double m1x1, double m1x2,
                        double m2x0, double m2x1, double m2x2)
        {
            m[0][0] = m0x0; m[0][1] = m0x1; m[0][2] = m0x2; m[0][3] = 0.0;
            m[1][0] = m1x0; m[1][1] = m1x1; m[1][2] = m1x2; m[1][3] = 0.0;
            m[2][0] = m2x0; m[2][1] = m2x1; m[2][2] = m2x2; m[2][3] = 0.0;
            m[3][0] = 0.0; m[3][1] = 0.0; m[3][2] = 0.0; m[3][3] = 1.0;

            return this;
        }

        public Mat4 Set(Mat4 other)
        {

            return Set(other.m[0][0], other.m[0][1], other.m[0][2], other.m[0][3],
                       other.m[1][0], other.m[1][1], other.m[1][2], other.m[1][3],
                       other.m[2][0], other.m[2][1], other.m[2][2], other.m[2][3],
                       other.m[3][0], other.m[3][1], other.m[3][2], other.m[3][3]);
        }

        public Mat4 Set(Vec3 v1, Vec3 v2, Vec3 v3)
        {
            return Set(v1.X, v1.Y, v1.Z,
                       v2.X, v2.Y, v2.Z,
                       v3.X, v3.Y, v3.Z);
        }

        public static Mat4 NewNull()
        {
            return new Mat4(0.0, 0.0, 0.0, 0.0,
                            0.0, 0.0, 0.0, 0.0,
                            0.0, 0.0, 0.0, 0.0,
                            0.0, 0.0, 0.0, 0.0);
        }

        public Mat4 Transpose()
        {
            return Set(m[0][0], m[1][0], m[2][0], m[3][0],
                       m[0][1], m[1][1], m[2][1], m[3][1],
                       m[0][2], m[1][2], m[2][2], m[3][2],
                       m[0][3], m[1][3], m[2][3], m[3][3]);
        }

        public Mat4 Opposed()
        {
            return Set(m[0][0] * -1.0, m[1][0] * -1.0, m[2][0] * -1.0, m[3][0] * -1.0,
                       m[0][1] * -1.0, m[1][1] * -1.0, m[2][1] * -1.0, m[3][1] * -1.0,
                       m[0][2] * -1.0, m[1][2] * -1.0, m[2][2] * -1.0, m[3][2] * -1.0,
                       m[0][3] * -1.0, m[1][3] * -1.0, m[2][3] * -1.0, m[3][3] * -1.0);
        }


        public Mat4 Identity()
        {
            return Set(1, 0, 0, 0,
                       0, 1, 0, 0,
                       0, 0, 1, 0,
                       0, 0, 0, 1);
        }

        public Mat4 Inverse()
        {
            double a = m[0][0] * m[1][1] - m[0][1] * m[1][0];
            double b = m[0][0] * m[1][2] - m[0][2] * m[1][0];
            double c = m[0][0] * m[1][3] - m[0][3] * m[1][0];
            double d = m[0][1] * m[1][2] - m[0][2] * m[1][1];
            double e = m[0][1] * m[1][3] - m[0][3] * m[1][1];
            double f = m[0][2] * m[1][3] - m[0][3] * m[1][2];
            double g = m[2][0] * m[3][1] - m[2][1] * m[3][0];
            double h = m[2][0] * m[3][2] - m[2][2] * m[3][0];
            double i = m[2][0] * m[3][3] - m[2][3] * m[3][0];
            double j = m[2][1] * m[3][2] - m[2][2] * m[3][1];
            double k = m[2][1] * m[3][3] - m[2][3] * m[3][1];
            double l = m[2][2] * m[3][3] - m[2][3] * m[3][2];
            double det = a * l - b * k + c * j + d * i - e * h + f * g;
            det = 1.0 / det;

            return Set((m[1][1] * l - m[1][2] * k + m[1][3] * j) * det,
                      (-m[0][1] * l + m[0][2] * k - m[0][3] * j) * det,
                       (m[3][1] * f - m[3][2] * e + m[3][3] * d) * det,
                      (-m[2][1] * f + m[2][2] * e - m[2][3] * d) * det,
                      (-m[1][0] * l + m[1][2] * i - m[1][3] * h) * det,
                       (m[0][0] * l - m[0][2] * i + m[0][3] * h) * det,
                      (-m[3][0] * f + m[3][2] * c - m[3][3] * b) * det,
                       (m[2][0] * f - m[2][2] * c + m[2][3] * b) * det,
                       (m[1][0] * k - m[1][1] * i + m[1][3] * g) * det,
                      (-m[0][0] * k + m[0][1] * i - m[0][3] * g) * det,
                       (m[3][0] * e - m[3][1] * c + m[3][3] * a) * det,
                      (-m[2][0] * e + m[2][1] * c - m[2][3] * a) * det,
                      (-m[1][0] * j + m[1][1] * h - m[1][2] * g) * det,
                       (m[0][0] * j - m[0][1] * h + m[0][2] * g) * det,
                      (-m[3][0] * d + m[3][1] * b - m[3][2] * a) * det,
                       (m[2][0] * d - m[2][1] * b + m[2][2] * a) * det);

        }

        public static Mat4 NewIdentity()
        {
            return new Mat4(1.0, 0.0, 0.0, 0.0,
                            0.0, 1.0, 0.0, 0.0,
                            0.0, 0.0, 1.0, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        public static Mat4 NewTranspose(Mat4 m)
        {
            return new Mat4(m).Transpose();
        }

        public static Mat4 NewOpposed(Mat4 m)
        {
            return new Mat4(m).Opposed();
        }

        public static Mat4 NewInverse(Mat4 m)
        {
            return new Mat4(m).Inverse();
        }

        public double Determinant
        {
            get
            {
                return (m[0][0] * m[1][1] - m[0][1] * m[1][0]) *
                       (m[2][2] * m[3][3] - m[2][3] * m[3][2]) +

                       (m[0][2] * m[1][0] - m[0][0] * m[1][2]) *
                       (m[2][1] * m[3][3] - m[2][3] * m[3][1]) +

                       (m[0][0] * m[1][3] - m[0][3] * m[1][0]) *
                       (m[2][1] * m[3][2] - m[2][2] * m[3][1]) +

                       (m[0][1] * m[1][2] - m[0][2] * m[1][1]) *
                       (m[2][0] * m[3][3] - m[2][3] * m[3][0]) +

                       (m[0][3] * m[1][1] - m[0][1] * m[1][3]) *
                       (m[2][0] * m[3][2] - m[2][2] * m[3][0]) +

                       (m[0][2] * m[1][3] - m[0][3] * m[1][2]) *
                       (m[2][0] * m[3][1] - m[2][1] * m[3][0]);
            }
        }

        public bool IsInvertible()
        {
            return Determinant != 0.0;
        }

        public double []this[int i]
        {
            get { return m[i]; }
        }
        
        public static Mat4 operator +(Mat4 m1, Mat4 m2)
        {
            return new Mat4(m1.m[0][0] + m2.m[0][0],
                             m1.m[0][1] + m2.m[0][1],
                             m1.m[0][2] + m2.m[0][2],
                             m1.m[0][3] + m2.m[0][3],

                             m1.m[1][0] + m2.m[1][0],
                             m1.m[1][1] + m2.m[1][1],
                             m1.m[1][2] + m2.m[1][2],
                             m1.m[1][3] + m2.m[1][3],

                             m1.m[2][0] + m2.m[2][0],
                             m1.m[2][1] + m2.m[2][1],
                             m1.m[2][2] + m2.m[2][2],
                             m1.m[2][3] + m2.m[2][3],

                             m1.m[3][0] + m2.m[3][0],
                             m1.m[3][1] + m2.m[3][1],
                             m1.m[3][2] + m2.m[3][2],
                             m1.m[3][3] + m2.m[3][3]);
        }

        public static Mat4 operator +(Mat4 m, double s)
        {
            return new Mat4(m.m[0][0] + s,
                            m.m[0][1] + s,
                            m.m[0][2] + s,
                            m.m[0][3] + s,

                            m.m[1][0] + s,
                            m.m[1][1] + s,
                            m.m[1][2] + s,
                            m.m[1][3] + s,

                            m.m[2][0] + s,
                            m.m[2][1] + s,
                            m.m[2][2] + s,
                            m.m[2][3] + s,

                            m.m[3][0] + s,
                            m.m[3][1] + s,
                            m.m[3][2] + s,
                            m.m[3][3] + s);
        }

        public static Mat4 operator +(double s, Mat4 m)
        {
            return new Mat4(s + m.m[0][0],
                            s + m.m[0][1],
                            s + m.m[0][2],
                            s + m.m[0][3],

                            s + m.m[1][0],
                            s + m.m[1][1],
                            s + m.m[1][2],
                            s + m.m[1][3],

                            s + m.m[2][0],
                            s + m.m[2][1],
                            s + m.m[2][2],
                            s + m.m[2][3],

                            s + m.m[3][0],
                            s + m.m[3][1],
                            s + m.m[3][2],
                            s + m.m[3][3]);
        }

        public static Mat4 operator -(Mat4 m1, Mat4 m2)
        {
            return new Mat4(m1.m[0][0] - m2.m[0][0],
                            m1.m[0][1] - m2.m[0][1],
                            m1.m[0][2] - m2.m[0][2],
                            m1.m[0][3] - m2.m[0][3],

                            m1.m[1][0] - m2.m[1][0],
                            m1.m[1][1] - m2.m[1][1],
                            m1.m[1][2] - m2.m[1][2],
                            m1.m[1][3] - m2.m[1][3],

                            m1.m[2][0] - m2.m[2][0],
                            m1.m[2][1] - m2.m[2][1],
                            m1.m[2][2] - m2.m[2][2],
                            m1.m[2][3] - m2.m[2][3],

                            m1.m[3][0] - m2.m[3][0],
                            m1.m[3][1] - m2.m[3][1],
                            m1.m[3][2] - m2.m[3][2],
                            m1.m[3][3] - m2.m[3][3]);
        }

        public static Mat4 operator -(Mat4 m, double s)
        {
            return new Mat4(m.m[0][0] - s,
                            m.m[0][1] - s,
                            m.m[0][2] - s,
                            m.m[0][3] - s,

                            m.m[1][0] - s,
                            m.m[1][1] - s,
                            m.m[1][2] - s,
                            m.m[1][3] - s,
                                      -
                            m.m[2][0] - s,
                            m.m[2][1] - s,
                            m.m[2][2] - s,
                            m.m[2][3] - s,

                            m.m[3][0] - s,
                            m.m[3][1] - s,
                            m.m[3][2] - s,
                            m.m[3][3] - s);
        }

        public static Mat4 operator -(double s, Mat4 m)
        {
            return new Mat4(s - m.m[0][0],
                            s - m.m[0][1],
                            s - m.m[0][2],
                            s - m.m[0][3],

                            s - m.m[1][0],
                            s - m.m[1][1],
                            s - m.m[1][2],
                            s - m.m[1][3],

                            s - m.m[2][0],
                            s - m.m[2][1],
                            s - m.m[2][2],
                            s - m.m[2][3],

                            s - m.m[3][0],
                            s - m.m[3][1],
                            s - m.m[3][2],
                            s - m.m[3][3]);
        }

        public static Mat4 operator *(Mat4 m1, Mat4 m2)
        {
            Mat4 aux = new Mat4();

            for (int c = 0; c < 4; c++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        aux.m[c][i] += m1.m[c][j] * m2.m[j][i];
                    }
                }
            }

            return aux;
        }

        public static Mat4 operator *(Mat4 m, double s)
        {
            return new Mat4(m.m[0][0] * s,
                            m.m[0][1] * s,
                            m.m[0][2] * s,
                            m.m[0][3] * s,

                            m.m[1][0] * s,
                            m.m[1][1] * s,
                            m.m[1][2] * s,
                            m.m[1][3] * s,

                            m.m[2][0] * s,
                            m.m[2][1] * s,
                            m.m[2][2] * s,
                            m.m[2][3] * s,

                            m.m[3][0] * s,
                            m.m[3][1] * s,
                            m.m[3][2] * s,
                            m.m[3][3] * s);
        }

        public static Mat4 operator *(double s, Mat4 m)
        {
            return new Mat4(s * m.m[0][0],
                            s * m.m[0][1],
                            s * m.m[0][2],
                            s * m.m[0][3],

                            s * m.m[1][0],
                            s * m.m[1][1],
                            s * m.m[1][2],
                            s * m.m[1][3],

                            s * m.m[2][0],
                            s * m.m[2][1],
                            s * m.m[2][2],
                            s * m.m[2][3],

                            s * m.m[3][0],
                            s * m.m[3][1],
                            s * m.m[3][2],
                            s * m.m[3][3]);
        }

        public static Mat4 operator /(Mat4 m, double s)
        {
            return new Mat4(m.m[0][0] / s,
                            m.m[0][1] / s,
                            m.m[0][2] / s,
                            m.m[0][3] / s,

                            m.m[1][0] / s,
                            m.m[1][1] / s,
                            m.m[1][2] / s,
                            m.m[1][3] / s,

                            m.m[2][0] / s,
                            m.m[2][1] / s,
                            m.m[2][2] / s,
                            m.m[2][3] / s,

                            m.m[3][0] / s,
                            m.m[3][1] / s,
                            m.m[3][2] / s,
                            m.m[3][3] / s);
        }

        public static Mat4 NewAffScale(Vec3 v)
        {
            return NewAffScale(v.X, v.Y, v.Z);
        }

        public static Mat4 NewAffScale(double x, double y, double z)
        {
            return new Mat4(x, 0.0, 0.0, 0.0,
                            0.0, y, 0.0, 0.0,
                            0.0, 0.0, z, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        public static Mat4 NewAffScale(double size)
        {
            return NewAffScale(size, size, size);
        }

        public static Mat4 NewRotationX(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);

            return new Mat4(1.0, 0.0, 0.0, 0.0,
                            0.0, cos, -sin, 0.0,
                            0.0, sin, cos, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        public static Mat4 NewRotationY(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);

            return new Mat4(cos, 0.0, sin, 0.0,
                            0.0, 1.0, 0.0, 0.0,
                           -sin, 0.0, cos, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        public static Mat4 NewRotationZ(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);

            return new Mat4(cos, -sin, 0.0, 0.0,
                            sin, cos, 0.0, 0.0,
                            0.0, 0.0, 1.0, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        public static Mat4 NewAffTranslation(Vec3 v)
        {
            return NewAffTranslation(v.X, v.Y, v.Z);
        }

        public static Mat4 NewAffTranslation(double x, double y, double z)
        {
            return new Mat4(1.0, 0.0, 0.0, x,
                            0.0, 1.0, 0.0, y,
                            0.0, 0.0, 1.0, z,
                            0.0, 0.0, 0.0, 1.0);
        }

        public override string ToString()
        {
            return "| " + m[0][0] + " " + m[0][1] + " " + m[0][2] + " " + m[0][3] + " |" +
                   "| " + m[1][0] + " " + m[1][1] + " " + m[1][2] + " " + m[1][3] + " |" +
                   "| " + m[2][0] + " " + m[2][1] + " " + m[2][2] + " " + m[2][3] + " |" +
                   "| " + m[3][0] + " " + m[3][1] + " " + m[3][2] + " " + m[3][3] + " |"  ;
        }
    }
}
