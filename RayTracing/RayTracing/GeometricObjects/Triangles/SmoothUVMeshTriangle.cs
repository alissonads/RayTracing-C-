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
    class SmoothUVMeshTriangle : SmoothMeshTriangle
    {
        public SmoothUVMeshTriangle() :
            base()
        {}
        
        public SmoothUVMeshTriangle(Mesh mesh, int i0, int i1, int i2) :
            base(mesh, i0, i1, i2)
        {}

        public SmoothUVMeshTriangle(SmoothUVMeshTriangle other) :
            base(other)
        {}

        public override GeometricObject Clone()
        {
            return new SmoothUVMeshTriangle(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
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

            if (beta < 0.0 || beta > 1.0)
                return false;

            double r = e * l - h * i;
            double e2 = a * n + d * q + c * r;
            double gamma = e2 * invDenom;

            if (gamma < 0.0 || gamma > 1.0)
                return false;

            if (beta + gamma > 1.0)
                return false;

            double e3 = a * p - b * r + d * s;
            double t = e3 * invDenom;

            if (t < kEpsilin)
                return false;

            tmin = t;
            sr.Normal = InterpolateNormal(beta, gamma);
            sr.LocalHitPoint = ray.HitPoint(t);
            sr.U = InterpolateU(beta, gamma);
            sr.V = InterpolateV(beta, gamma);

            return true;
        }
    }
}
