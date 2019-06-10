using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;
using RayTracing.Util;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Base
{
    abstract class GeometricObject
    {
        protected IMaterial material;
        protected Vec3 color;
        protected bool shadows;
        
        public virtual IMaterial Material
        {
            get { return material; }
            set
            {
                material = value;
            }
        }

        public Vec3 Color
        {
            get { return color; }
            set { color = value; }
        }

        public bool Shadows
        {
            get { return shadows; }
            set
            {
                shadows = value;
            }
        }

        public virtual BBox BoundingBox
        {
            get { return new BBox(); }
            set { }
        }

        public GeometricObject()
        {
            color = new Vec3();
            shadows = true;
        }

        public GeometricObject(Vec3 color)
        {
            this.color = color;
            shadows = true;
        }

        public GeometricObject(GeometricObject other)
        {
            color = other.color.Clone();
            shadows = other.shadows;

            if(other.material != null)
                material = other.material.Clone();
        }

        public abstract GeometricObject Clone();

        public abstract bool Hit(Ray ray, ref double tmin, ShadeRec sr);

        public abstract bool ShadowHit(Ray ray, ref double tmin);

    }
}
