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
    class DiskLamp : GeometricObject, IEmissiveObject
    {
        private Disk disk;
        private Sampler sampler;
        private Vec3 up;
        private Vec3 u, v, w;
        private double area;
        private IEmissiveMaterial emissive;

        public Disk Disk
        {
            set
            {
                disk = value;
                material = value.Material;
                ComputeUVW();
            }
        }

        public Sampler Sampler
        {
            get { return sampler; }
            set
            {
                sampler = value;
                sampler.MapSamplesToUnitDisk();
            }
        }

        public override IMaterial Material
        {
            get { return material; }

            set
            {
                material = value;
                disk.Material = value;
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

        public DiskLamp(Disk disk) :
            base()
        {
            this.disk = disk;
            material = disk.Material;
            up = new Vec3(0, 1, 0);
            ComputeUVW();
            area = MathUtils.PI * disk.Radius * disk.Radius;
            shadows = false;
        }

        public DiskLamp(Disk disk, Emissive emissive ) :
            base()
        {
            this.disk = disk;
            up = new Vec3(0, 1, 0);
            ComputeUVW();
            area = MathUtils.PI * disk.Radius * disk.Radius;
            material = disk.Material = emissive;
            this.emissive = emissive;
            shadows = false;
        }

        public DiskLamp(DiskLamp other) :
            base()
        {
            disk = (Disk)other.disk.Clone();
            material = disk.Material;
            up = other.up.Clone();
            u = other.u.Clone();
            v = other.v.Clone();
            w = other.w.Clone();
            area = MathUtils.PI * disk.Radius * disk.Radius;

            if (other.sampler != null)
            {
                sampler = other.sampler.Clone();
                sampler.MapSamplesToUnitDisk();
            }
            if (other.emissive != null)
                emissive = other.emissive.Clone();
        }

        public override GeometricObject Clone()
        {
            return new DiskLamp(this);
        }

        public IEmissiveObject CloneEmissive()
        {
            return new DiskLamp(this);
        }

        public Vec3 GetNormal(Vec3 sp)
        {
            return disk.Normal;
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            return disk.Hit(ray, ref tmin, sr);
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            return disk.ShadowHit(ray, ref tmin);
        }

        public float PDF(ShadeRec sr)
        {
            return (float)(1 / area);
        }

        public Vec3 Sample()
        {
            Vec2 d = sampler.SampleUnitDisk() * disk.Radius;
            
            return disk.Center + (u * d.X) + (v * d.Y);
        }

        public void ComputeUVW()
        {
            w = disk.Normal.Clone();
            w.Normalize();
            u = Vec3.Cross(up, w);
            u.Normalize();
            v = Vec3.Cross(w, u);

            Vec3 normal = disk.Normal;

            if(normal.X == up.X && normal.Y == up.Y && normal.Z == up.Z)
            {
                u = new Vec3(0, 0, 1);
                v = new Vec3(1, 0, 0);
                w = new Vec3(0, 1, 0);
            }
            if (normal.X == up.X && normal.Y == -up.Y && normal.Z == up.Z)
            {
                u = new Vec3(1, 0, 0);
                v = new Vec3(0, 0, 1);
                w = new Vec3(0, -1, 0);
            }
        }
    }
}
