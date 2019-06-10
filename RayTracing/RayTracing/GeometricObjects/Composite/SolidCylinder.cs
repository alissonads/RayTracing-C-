using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;
using RayTracing.GeometricObjects.Part;

namespace RayTracing.GeometricObjects.Composite
{
    class SolidCylinder : Compound
    {
        private BBox bbox;

        public override BBox BoundingBox
        {
            get { return bbox; }

            set
            {
                bbox = value;
            }
        }

        public SolidCylinder() : 
            this(-1, 1, 1)
        {}

        public SolidCylinder(float bottom, float top, float radius) : 
            this(bottom, top, radius, null)
        {}

        public SolidCylinder(float bottom, float top, float radius, IMaterial material) :
            base()
        {
            //top
            AddObject(new Disk(new Vec3(0, top, 0),
                               new Vec3(0, 1, 0),
                               radius));
            //wall
            //AddObject(new OpenCylinder(bottom, top, radius));
            AddObject(new ConvexPartCylinder(bottom, top, radius));
            //bottom
            AddObject(new Disk(new Vec3(0, bottom, 0),
                               new Vec3(0, -1, 0),
                               radius));
            bbox = new BBox(-radius, radius, bottom, top, -radius, radius);

            Material = material;
        }

        public SolidCylinder(SolidCylinder other) : 
            base(other)
        {
            if (other.bbox != null)
                bbox = other.bbox.Clone();
        }
        
        public void SetBottomMaterial(IMaterial material)
        {
            objects[2].Material = material;
        }

        public void SetWallMaterial(IMaterial material)
        {
            objects[1].Material = material;
        }

        public void SetTopMaterial(IMaterial material)
        {
            objects[0].Material = material;
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            if(bbox.Hit(ray))
                return base.Hit(ray, ref tmin, sr);

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows || !bbox.Hit(ray))
                return false;

            return base.ShadowHit(ray, ref tmin);;
        }
    }
}
