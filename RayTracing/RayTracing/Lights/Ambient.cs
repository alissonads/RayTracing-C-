using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Lights.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Lights
{
    class Ambient : Light
    {
        private float ls;
        private Vec3 color;

        public float ScaleRadiance
        {
            get { return ls; }
            set
            {
                ls = value;
            }
        }

        public Vec3 Color
        {
            get { return color; }
            set
            {
                color = value;
            }
        }

        public Ambient() :
            base(false)
        {
            ls = 1.0f;
            color = new Vec3(1.0);
        }

        public Ambient(Vec3 color) :
            this(color, 1.0f)
        {}

        public Ambient(Vec3 color, float ls) :
            base(false)
        {
            this.ls = ls;
            this.color = color;
        }

        public Ambient(Ambient other) :
            base(other)
        {
            ls = other.ls;
            color = other.color.Clone();
        }

        public void SetColor(float r, float g, float b)
        {
            color.Set(r, g, b);
        }

        public void SetColor(float rgb)
        {
            color.Set(rgb);
        }

        public override Light Clone()
        {
            return new Ambient(this);
        }

        public override Vec3 GetDirection(ShadeRec sr)
        {
            return new Vec3();
        }

        public override Vec3 L(ShadeRec sr)
        {
            return ls * color;
        }
    }
}
