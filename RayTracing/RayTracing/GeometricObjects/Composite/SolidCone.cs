using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.GeometricObjects.Composite
{
    class SolidCone : Compound
    {
        private BBox bbox;

        public override BBox BoundingBox
        {
            get
            { return bbox; }

            set
            {
                bbox = value;
            }
        }

        public SolidCone() :
            this(2, 1)
        {}

        public SolidCone(double h, double r) :
            this(new Vec3(), h, r)
        {}

        public SolidCone(Vec3 center, double h, double r) :
            this(center, h, r, null)
        {}

        public SolidCone(Vec3 center, double h, double r, IMaterial material) :
            base()
        {
            OpenCone oc = new OpenCone(center, h, r);
            Disk bottom = new Disk(center, new Vec3(0, -1, 0), r);
            AddObject(oc);
            AddObject(bottom);

            double x0 = center.X - r;
            double x1 = center.X + r;
            double y0 = center.Y;
            double y1 = center.Y + h;
            double z0 = center.Z - r;
            double z1 = center.Z + r;

            bbox = new BBox(x0, x1, y0, y1, z0, z1);

            Material = material;
        }

        public SolidCone(SolidCone other) :
            base(other)
        {
            if(other.bbox != null)
                bbox = other.bbox.Clone();
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            if(bbox.Hit(ray))
                return base.Hit(ray, ref tmin, sr);

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows || bbox.Hit(ray))
                return false;

            return base.ShadowHit(ray, ref tmin);
        }

        public void SetBodyMaterial(IMaterial material)
        {
            objects[0].Material = material;
        }

        public void SetBaseMaterial(IMaterial material)
        {
            objects[1].Material = material;
        }
    }
}
