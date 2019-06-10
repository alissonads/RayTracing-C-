using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util
{
    using Math;

    sealed class Ray
    {
        private Vec3 o;
        private Vec3 d;

        public Ray()
        {
            o = new Vec3();
            d = new Vec3(0.0, 0.0, 1.0);
        }

        public Ray(Vec3 o, Vec3 d)
        {
            Set(o, d);
        }

        public Ray(Ray other)
        {
            o = other.o.Clone();
            d = other.d.Clone();
        }

        public Ray Clone()
        {
            return new Ray(this);
        }

        public Ray Set(Vec3 o, Vec3 d)
        {
            this.o = o;
            this.d = d;

            return this;
        }

        public Vec3 O
        {
            get { return o; }
            set { o = value; }
        }

        public Vec3 D
        {
            get { return d; }
            set { d = value; }
        }

        public Vec3 HitPoint(double t)
        {
            return (o + t * d);
        }
    }
}
