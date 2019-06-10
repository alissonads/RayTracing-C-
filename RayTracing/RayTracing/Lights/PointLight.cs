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
    class PointLight : Light
    {
        private float ls;
        private Vec3 color;
        private Vec3 location;

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

        public Vec3 Location
        {
            get { return location; }
            set
            {
                location = value;
            }
        }

        public PointLight() :
            this(new Vec3(), new Vec3(1.0), 1.0f)
        {}

        public PointLight(Vec3 location, Vec3 color, float ls) :
            base()
        {
            this.location = location;
            this.color = color;
            this.ls = ls;
        }

        public PointLight(PointLight other) : 
            base(other)
        {
            ls = other.ls;
            color = other.color.Clone();
            location = other.location.Clone();
        }

        public void SetColor(float r, float g, float b)
        {
            color.Set(r, g, b);
        }

        public void SetColor(float rgb)
        {
            color.Set(rgb);
        }

        public void SetLocation(float x, float y, float z)
        {
            location.Set(x, y, z);
        }

        public override Light Clone()
        {
            return new PointLight(this);
        }

        public override Vec3 GetDirection(ShadeRec sr)
        {
            return (location - sr.HitPoint).Normalize();
        }

        public override Vec3 L(ShadeRec sr)
        {
            return ls * color;
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            double d = (float)(location - ray.O).Size;
            
            return sr.World.ShadowHitObjects(ray, d);
        }
    }
}
