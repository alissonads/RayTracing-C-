using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Base;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Samplers.Base;
using RayTracing.Materials;

namespace RayTracing.GeometricObjects.Lamps
{
    class RectangularLamp : GeometricObject, IEmissiveObject
    {
        private Rectangle rectangle;
        private Sampler sampler;
        private IEmissiveMaterial emissive;

        public Rectangle Rectangle
        {
            set
            {
                rectangle = value;
                material = value.Material;
            }
        }

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
            }
        }

        public override IMaterial Material
        {
            get { return material; }

            set
            {
                material = value;
                rectangle.Material = value;
            }
        }

        public IEmissiveMaterial EmissiveMaterial
        {
            get { return emissive; }

            set
            {
                emissive = value;
            }
        }

        public RectangularLamp(Rectangle rectangle) :
            base()
        {
            this.rectangle = rectangle;
            material = rectangle.Material;
            shadows = false;
        }

        public RectangularLamp(Rectangle rectangle,
                               Emissive emissive) :
            base()
        {
            this.rectangle = rectangle;
            material = emissive;
            this.emissive = emissive;
            shadows = false;
        }

        public RectangularLamp(RectangularLamp other) :
            base(other)
        {
            rectangle = (Rectangle)other.rectangle.Clone();
            material = rectangle.Material;

            if(other.emissive != null)
                emissive = other.emissive.Clone();

            if (other.sampler != null)
                sampler = other.sampler.Clone();
        }
        
        public override GeometricObject Clone()
        {
            return new RectangularLamp(this);
        }

        public Vec3 GetNormal(Vec3 sp)
        {
            return rectangle.Normal;
        }
        
        public float PDF(ShadeRec sr)
        {
            return 1.0f / (float)(rectangle.A.Size *
                                  rectangle.B.Size);
        }

        public Vec3 Sample()
        {
            Vec2 samplePoint = sampler.SampleUnitSquare();
            return (rectangle.P0 + samplePoint.X * 
                    rectangle.A +  samplePoint.Y * 
                    rectangle.B);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            return rectangle.Hit(ray, ref tmin, sr);
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            return rectangle.ShadowHit(ray, ref tmin);
        }

        public IEmissiveObject CloneEmissive()
        {
            return new RectangularLamp(this);
        }
    }
}
