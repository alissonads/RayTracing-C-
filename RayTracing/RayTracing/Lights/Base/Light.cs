using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;
using RayTracing.Util;

namespace RayTracing.Lights.Base
{
    abstract class Light
    {
        protected bool shadows;

        public bool Shadows
        {
            get { return shadows; }
            set
            {
                shadows = value;
            }
        }

        public Light()
        {
            shadows = true;
        }

        public Light(bool shadows)
        {
            this.shadows = shadows;
        }

        public Light(Light other)
        {
            shadows = other.shadows;
        }

        public abstract Light Clone();

        public abstract Vec3 GetDirection(ShadeRec sr);

        public abstract Vec3 L(ShadeRec sr);

        public virtual bool InShadow(Ray ray, ShadeRec sr)
        {
            return false;
        }

        public virtual double G(ShadeRec sr)
        {
            return 1.0;
        }

        public virtual float PDF(ShadeRec sr)
        {
            return 1.0f;
        }

    }
}
