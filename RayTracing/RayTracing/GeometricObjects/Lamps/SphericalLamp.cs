using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Samplers.Base;
using RayTracing.Materials;

namespace RayTracing.GeometricObjects.Lamps
{
    class SphericalLamp : GeometricObject, IEmissiveObject
    {
        private Sphere sphere;
        private Sampler sampler;
        private IEmissiveMaterial emissive;

        public Sphere Sphere
        {
            set
            {
                sphere = value;
                material = sphere.Material;
            }
        }

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                sampler.MapSamplesToSphere();
            }
        }

        public override IMaterial Material
        {
            get { return material; }

            set
            {
                material = value;
                sphere.Material = value;
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

        public SphericalLamp(Sphere sphere) :
            base()
        {
            this.sphere = sphere;
            material = sphere.Material;
            shadows = false;
        }

        public SphericalLamp(Sphere sphere, Emissive emissive) :
            base()
        {
            this.sphere = sphere;
            material = emissive;
            sphere.Material = emissive;
            this.emissive = emissive;
            shadows = false;
        }

        public SphericalLamp(SphericalLamp other) :
            base(other)
        {
            sphere = (Sphere)other.sphere.Clone();
            material = sphere.Material;

            if (other.sampler != null)
                sampler = other.sampler.Clone();
            if (other.emissive != null)
                emissive = other.emissive.Clone();
        }

        public override GeometricObject Clone()
        {
            return new SphericalLamp(this);
        }

        public IEmissiveObject CloneEmissive()
        {
            return new SphericalLamp(this);
        }

        public Vec3 GetNormal(Vec3 sp)
        {
            Vec3 n = sphere.Center - sp;
            n.Normalize();
            return n;
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            return sphere.Hit(ray, ref tmin, sr);
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            return sphere.ShadowHit(ray, ref tmin);
        }

        public float PDF(ShadeRec sr)
        {
            return (float)(1.0 / (4.0 * MathUtils.PI * sphere.Radius * sphere.Radius));
        }

        public Vec3 Sample()
        {
            return sampler.SampleSphere() * sphere.Radius + sphere.Center;
        }
    }
}
