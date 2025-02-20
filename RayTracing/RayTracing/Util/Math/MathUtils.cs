﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util.Math
{
    sealed class MathUtils
    {
        
        internal static double PI { get { return System.Math.PI; } }

        internal static double TwoPI { get { return 2 * PI; } }

        internal static double InvPI { get { return 1.0 / PI; } }

        internal static double Inv2PI { get { return 1.0 / TwoPI; } }

        internal static double HugeValue { get { return 1.0e10; } }

        internal static double TORAD = System.Math.PI / 180.0;
        internal static double TODEG = 180.0 / System.Math.PI;
        internal static double EQN_EPS = 1e-90;

        internal static double ToRadians(double degress)
        {
            return degress * TORAD;
        }

        internal static double ToDegrees(double radians)
        {
            return radians * TODEG;
        }

        internal static int SolveQuartic(double []c, double []s)
        {
            double []coeffs = new double[4];
            double z, u, v, sub;
            double A, B, C, D;
            double sq_A, p, q, r;
            int i, num;

            /* normal form: x^4 + Ax^3 + Bx^2 + Cx + D = 0 */

            A = c[3] / c[4];
            B = c[2] / c[4];
            C = c[1] / c[4];
            D = c[0] / c[4];

            /*  substitute x = y - A/4 to eliminate cubic term:
            x^4 + px^2 + qx + r = 0 */

            sq_A = A * A;
            p = -3.0 / 8 * sq_A + B;
            q = 1.0 / 8 * sq_A * A - 1.0 / 2 * A * B + C;
            r = -3.0 / 256 * sq_A * sq_A + 1.0 / 16 * sq_A * B - 1.0 / 4 * A * C + D;

            if (IsZero(r))
            {
                /* no absolute term: y(y^3 + py + q) = 0 */

                coeffs[0] = q;
                coeffs[1] = p;
                coeffs[2] = 0;
                coeffs[3] = 1;

                num = SolveCubic(coeffs, s);

                s[num++] = 0;
            }
            else
            {
                /* solve the resolvent cubic ... */

                coeffs[0] = 1.0 / 2 * r * p - 1.0 / 8 * q * q;
                coeffs[1] = -r;
                coeffs[2] = -1.0 / 2 * p;
                coeffs[3] = 1;

                SolveCubic(coeffs, s);

                /* ... and take the one real solution ... */

                z = s[0];

                /* ... to build two quadric equations */

                u = z * z - r;
                v = 2 * z - p;

                if (IsZero(u))
                    u = 0;
                else if (u > 0)
                    u = System.Math.Sqrt(u);
                else
                    return 0;

                if (IsZero(v))
                    v = 0;
                else if (v > 0)
                    v = System.Math.Sqrt(v);
                else
                    return 0;

                coeffs[0] = z - u;
                coeffs[1] = q < 0 ? -v : v;
                coeffs[2] = 1;

                num = SolveQuadric(coeffs, s);

                coeffs[0] = z + u;
                coeffs[1] = q < 0 ? v : -v;
                coeffs[2] = 1;

                double []s2 = { s[num], s[num + 1] };
                num += SolveQuadric(coeffs, s2);
            }

            /* resubstitute */

            sub = 1.0 / 4 * A;

            for (i = 0; i < num; ++i)
                s[i] -= sub;

            return num;
        }

        internal static int SolveQuadric(double []c, double []s)
        {
            double p, q, D;

            /* normal form: x^2 + px + q = 0 */

            p = c[1] / (2 * c[2]);
            q = c[0] / c[2];

            D = p * p - q;

            if (IsZero(D))
            {
                s[0] = -p;
                return 1;
            }
            else if (D > 0)
            {
                double sqrt_D = System.Math.Sqrt(D);

                s[0] = sqrt_D - p;
                s[1] = -sqrt_D - p;
                return 2;
            }
            
            return 0;
        }

        internal static int SolveCubic(double []c, double[] s)
        {
            int i, num;
            double sub;
            double A, B, C;
            double sq_A, p, q;
            double cb_p, D;

            /* normal form: x^3 + Ax^2 + Bx + C = 0 */

            A = c[2] / c[3];
            B = c[1] / c[3];
            C = c[0] / c[3];

            /*  substitute x = y - A/3 to eliminate quadric term:
            x^3 +px + q = 0 */

            sq_A = A * A;
            p = 1.0 / 3 * (-1.0 / 3 * sq_A + B);
            q = 1.0 / 2 * (2.0 / 27 * A * sq_A - 1.0 / 3 * A * B + C);

            /* use Cardano's formula */

            cb_p = p * p * p;
            D = q * q + cb_p;

            if (IsZero(D))
            {
                if (IsZero(q))
                { /* one triple solution */
                    s[0] = 0;
                    num = 1;
                }
                else
                { /* one single and one double solution */
                    double u = Cbrt(-q);
                    s[0] = 2 * u;
                    s[1] = -u;
                    num = 2;
                }
            }
            else if (D < 0)
            { /* Casus irreducibilis: three real solutions */
                double phi = 1.0 / 3 * System.Math.Acos(-q / System.Math.Sqrt(-cb_p));
                double t = 2 * System.Math.Sqrt(-p);

                s[0] = t * System.Math.Cos(phi);
                s[1] = -t * System.Math.Cos(phi + PI / 3);
                s[2] = -t * System.Math.Cos(phi - PI / 3);
                num = 3;
            }
            else
            { /* one real solution */
                double sqrt_D = System.Math.Sqrt(D);
                double u = Cbrt(sqrt_D - q);
                double v = -Cbrt(sqrt_D + q);

                s[0] = u + v;
                num = 1;
            }

            /* resubstitute */

            sub = 1.0 / 3 * A;

            for (i = 0; i < num; ++i)
                s[i] -= sub;

            return num;
        }

        private static double Cbrt(double x)
        {
            return (x > 0.0) ? System.Math.Pow(x, 1.0 / 3.0) :
                   (x < 0.0) ? -System.Math.Pow(-x, 1.0 / 3.0) : 0.0;
        }

        internal static bool IsZero(double x)
        {
            return (x > -EQN_EPS && x < EQN_EPS);
        }

        internal static double Clamp(double x, double min, double max)
        {
            return (x < min) ? min : (x > max) ? max : x;
        }

        internal static Vec3 TransformPoint(Mat4 m, Vec3 p)
        {
            return new Vec3(m[0][0] * p.X + m[0][1] * p.Y + m[0][2] * p.Z + m[0][3],
                            m[1][0] * p.X + m[1][1] * p.Y + m[1][2] * p.Z + m[1][3],
                            m[2][0] * p.X + m[2][1] * p.Y + m[2][2] * p.Z + m[2][3]);
        }

        internal static Vec3 TransformDirection(Mat4 m, Vec3 d)
        {
	        return new Vec3(m[0][0] * d.X + m[0][1] * d.Y + m[0][2] * d.Z,
					        m[1][0] * d.X + m[1][1] * d.Y + m[1][2] * d.Z,
					        m[2][0] * d.X + m[2][1] * d.Y + m[2][2] * d.Z);
        }

        internal static Vec3 TransformNormal(Mat4 m, Vec3 n)
        {
	        return new Vec3(m[0][0] * n.X + m[1][0] * n.Y + m[2][0] * n.Z,
					        m[0][1] * n.X + m[1][1] * n.Y + m[2][1] * n.Z,
					        m[0][2] * n.X + m[1][2] * n.Y + m[2][2] * n.Z)
			       .Normalize();
        }
    }

}
