using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Primitives
{
    class Torus : GeometricObject
    {
        private double a, b;
        private BBox bbox;
        private static double kEpsilon = 0.0001;

        public Torus() :
            base()
        {
            a = 2.0;
            b = 0.5;
            bbox = new BBox(-a - b, a + b, -b, b, -a - b, a + b);
        }

        public Torus(double a, double b) :
            this(a, b, null)
        {
            this.a = a;
            this.b = b;
            bbox = new BBox(-a - b, a + b, -b, b, -a - b, a + b);
        }

        public Torus(double a, double b, IMaterial material) :
            base()
        {
            this.a = a;
            this.b = b;
            bbox = new BBox(-a - b, a + b, -b, b, -a - b, a + b);
            this.material = material;
        }

        public Torus(Torus other) :
            base(other)
        {
            a = other.a;
            b = other.b;
            bbox = other.bbox.Clone();
        }
        public Vec3 ComputeNormal(Vec3 p)
        {
            double paramSquared = a * a + b * b;

            double x = p.X;
            double y = p.Y;
            double z = p.Z;
            double sumSquared = x * x + y * y + z * z;

            Vec3 normal = new Vec3(4.0 * x * (sumSquared - paramSquared),
                                   4.0 * y * (sumSquared - paramSquared + 2.0 * a * a),
                                   4.0 * z * (sumSquared - paramSquared));
            normal.Normalize();

            return normal;
        }

        public override GeometricObject Clone()
        {
            return new Torus(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            if (!bbox.Hit(ray))
                return false;

            double x1 = ray.O.X;
            double y1 = ray.O.Y;
            double z1 = ray.O.Z;
            double d1 = ray.D.X;
            double d2 = ray.D.Y;
            double d3 = ray.D.Z;

            double []coeffs = new double[5];
            double []roots = new double[4];

            double sumDsqrd = ray.D.SizeSQR;
            double e = ray.O.SizeSQR - a * a - b * b;
            double f = ray.O.Dot(ray.D);
            double fourAsqrd = 4.0 * a * a;

            coeffs[0] = e * e - fourAsqrd * (b * b - y1 * y1);
            coeffs[1] = 4.0 * f * e + 2.0 * fourAsqrd * y1 * d2;
            coeffs[2] = 2.0 * sumDsqrd * e + 4.0 * f * f + fourAsqrd * d2 * d2;
            coeffs[3] = 4.0 * sumDsqrd * f;
            coeffs[4] = sumDsqrd * sumDsqrd;

            // find the roots

            int numRealRoots = MathUtils.SolveQuartic(coeffs, roots);

            bool intersected = false;
            double t = MathUtils.HugeValue;

            if (numRealRoots == 0)
                return false;

            // find the smallest root greater than kEpsilon, if any

            for(int j = 0; j < numRealRoots; j++)
                if(roots[j] > kEpsilon)
                {
                    intersected = true;
                    if (roots[j] < t)
                        t = roots[j];
                }

            if (!intersected)
                return false;

            tmin = t;
            sr.LocalHitPoint = ray.HitPoint(t);
            sr.Normal = ComputeNormal(sr.LocalHitPoint);

            return true;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows || !bbox.Hit(ray))
                return false;

            double x1 = ray.O.X;
            double y1 = ray.O.Y;
            double z1 = ray.O.Z;
            double d1 = ray.D.X;
            double d2 = ray.D.Y;
            double d3 = ray.D.Z;

            double[] coeffs = new double[5];
            double[] roots = new double[4];

            double sumDsqrd = ray.D.SizeSQR;
            double e = ray.O.SizeSQR - a * a - b * b;
            double f = ray.O.Dot(ray.D);
            double fourAsqrd = 4.0 * a * a;

            coeffs[0] = e * e - fourAsqrd * (b * b - y1 * y1);
            coeffs[1] = 4.0 * f * e + 2.0 * fourAsqrd * y1 * d2;
            coeffs[2] = 2.0 * sumDsqrd * e + 4.0 * f * f + fourAsqrd * d2 * d2;
            coeffs[3] = 4.0 * sumDsqrd * f;
            coeffs[4] = sumDsqrd * sumDsqrd;

            // find the roots

            int numRealRoots = MathUtils.SolveQuartic(coeffs, roots);

            bool intersected = false;
            double t = MathUtils.HugeValue;

            if (numRealRoots == 0)
                return false;

            // find the smallest root greater than kEpsilon, if any

            for (int j = 0; j < numRealRoots; j++)
                if (roots[j] > kEpsilon)
                {
                    intersected = true;
                    if (roots[j] < t)
                        t = roots[j];
                }

            if (!intersected)
                return false;

            tmin = t;

            return true;
        }
    }
}
