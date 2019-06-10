using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.GeometricObjects.Triangles
{
    abstract class MeshTriangle : GeometricObject
    {
        public Mesh Mesh;
        public int Index0, Index1, Index2;
        public Vec3 Normal;
        public float Area;
        protected static double kEpsilin = 0.0001;

        public override BBox BoundingBox
        {
            get
            {
                double delta = 0.0001;

                Vec3 v0 = Mesh.Vertices[Index0];
                Vec3 v1 = Mesh.Vertices[Index1];
                Vec3 v2 = Mesh.Vertices[Index2];

                double x0 = Math.Min(Math.Min(v0.X, v1.X), v2.X) - delta;
                double x1 = Math.Max(Math.Max(v0.X, v1.X), v2.X) + delta;
                double y0 = Math.Min(Math.Min(v0.Y, v1.Y), v2.Y) - delta;
                double y1 = Math.Max(Math.Max(v0.Y, v1.Y), v2.Y) + delta;
                double z0 = Math.Min(Math.Min(v0.Z, v1.Z), v2.Z) - delta;
                double z1 = Math.Max(Math.Max(v0.Z, v1.Z), v2.Z) + delta;

                return new BBox(x0, x1, y0, y1, z0, z1);
            }
        }

        public MeshTriangle() :
            this(null, 0, 0, 0)
        {}

        public MeshTriangle(Mesh mesh, int i0, int i1, int i2) :
            base()
        {
            Mesh = mesh;
            Index0 = i0;
            Index1 = i1;
            Index2 = i2;
            Normal = new Vec3();
            Area = 0.0f;
        }

        public MeshTriangle(MeshTriangle other) :
            base(other)
        {
            Mesh = other.Mesh;
            Index0 = other.Index0;
            Index1 = other.Index1;
            Index2 = other.Index2;
            Normal = other.Normal;
            Area = other.Area;
        }

        public void ComputeNormal(bool reverseNormal)
        {
            Normal = (Mesh.Vertices[Index1] - Mesh.Vertices[Index0])
                      .Cross(Mesh.Vertices[Index2] - Mesh.Vertices[Index0]);
            Normal.Normalize();

            if (reverseNormal)
                Normal.Neg();
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            Vec3 v0 = Mesh.Vertices[Index0];
            Vec3 v1 = Mesh.Vertices[Index1];
            Vec3 v2 = Mesh.Vertices[Index2];

            double a = v0.X - v1.X, b = v0.X - v2.X, c = ray.D.X, d = v0.X - ray.O.X;
            double e = v0.Y - v1.Y, f = v0.Y - v2.Y, g = ray.D.Y, h = v0.Y - ray.O.Y;
            double i = v0.Z - v1.Z, j = v0.Z - v2.Z, k = ray.D.Z, l = v0.Z - ray.O.Z;

            double m = f * k - g * j, n = h * k - g * l, p = f * l - h * j;
            double q = g * i - e * k, s = e * j - f * i;

            double invDenom = 1.0 / (a * m + b * q + c * s);

            double e1 = d * m - b * n - c * p;
            double beta = e1 * invDenom;

            if (beta < 0.0)
                return false;

            double r = e * l - h * i;
            double e2 = a * n + d * q + c * r;
            double gamma = e2 * invDenom;

            if (gamma < 0.0)
                return false;

            if (beta + gamma > 1.0)
                return false;

            double e3 = a * p - b * r + d * s;
            double t = e3 * invDenom;

            if (t < kEpsilin)
                return false;

            tmin = t;

            return true;
        }

        protected double InterpolateU(double beta, double gamma)
        {
            return ((1 - beta - gamma) * Mesh.U[Index0] +
                    beta * Mesh.U[Index1] +
                    gamma * Mesh.U[Index2]);
        }

        protected double InterpolateV(double beta, double gamma)
        {
            return ((1 - beta - gamma) * Mesh.V[Index0] +
                    beta * Mesh.V[Index1] +
                    gamma * Mesh.V[Index2]);
        }
    }
}
