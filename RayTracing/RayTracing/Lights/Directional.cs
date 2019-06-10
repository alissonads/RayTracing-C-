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
    class Directional : Light
    {
        private float ls;
        private Vec3 color;
        private Vec3 dir;

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

        public Vec3 Direcition
        {
            get { return dir; }
            set
            {
                dir = value;
                dir.Normalize();
            }
        }

        public Directional() :
            base()
        {
            ls = 1.0f;
            color = new Vec3(1.0);
            dir = new Vec3(0, 1, 0);
        }

        public Directional(Vec3 color, float ls, Vec3 direction) :
            base()
        {
            this.color = color;
            this.ls = ls;
            dir = direction;
        }

        public Directional(Directional other) :
            base(other)
        {
            ls = other.ls;
            color = other.color.Clone();
            dir = other.color.Clone();
        }

        public void SetColor(float r, float g, float b)
        {
            color.Set(r, g, b);
        }

        public void SetColor(float rgb)
        {
            color.Set(rgb);
        }

        public void SetDirection(float x, float y, float z)
        {
            dir.Set(x, y, z);
            dir.Normalize();
        }

        public override Light Clone()
        {
            return new Directional(this);
        }

        public override Vec3 GetDirection(ShadeRec sr)
        {
            return dir; 
        }

        public override Vec3 L(ShadeRec sr)
        {
            return ls * color;
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            return sr.World.ShadowHitObjects(ray, MathUtils.HugeValue);
        }
    }
}
