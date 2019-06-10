using RayTracing.GeometricObjects.Part;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.GeometricObjects.Composite
{
    class BeveledCylinder : Compound
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

        public BeveledCylinder(float bottom,
                               float top,
                               float radius,
                               float bevelRadius) :
            this(bottom, top, radius, bevelRadius, null)
        {}

        public BeveledCylinder(float bottom,
                               float top,
                               float radius,
                               float bevelRadius,
                               IMaterial material) :
            base()
        {
            AddObject(new Disk(new Vec3(0, top, 0),
                               new Vec3(0, 1, 0),
                               radius - bevelRadius));

            Instance topBevel = new Instance(new Torus(radius - bevelRadius,
                                                       bevelRadius));
            topBevel.Translate(0, top - bevelRadius, 0);
            AddObject(topBevel);

            //AddObject(new OpenCylinder(bottom + bevelRadius,
            //                           top - bevelRadius,
            //                           radius));
            AddObject(new ConvexPartCylinder(bottom + bevelRadius,
                                             top - bevelRadius,
                                             radius));

            Instance bottomBevel = new Instance(new Torus(radius - bevelRadius,
                                                          bevelRadius));
            bottomBevel.Translate(0, bottom + bevelRadius, 0);
            AddObject(bottomBevel);

            AddObject(new Disk(new Vec3(0, bottom, 0),
                               new Vec3(0, -1, 0),
                               radius - bevelRadius));

            bbox = new BBox(-radius, radius, bottom, top, -radius, radius);
            Material = material;
        }

        public void SetTopMaterial(IMaterial material)
        {
            objects[0].Material = material;
        }

        public void SeTopBorderMaterial(IMaterial material)
        {
            objects[1].Material = material;
        }

        public void SetWallMaterial(IMaterial material)
        {
            objects[2].Material = material;
        }

        public void SetBottomBorderMaterial(IMaterial material)
        {
            objects[3].Material = material;
        }

        public void SetBottomMaterial(IMaterial material)
        {
            objects[4].Material = material;
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

            return base.ShadowHit(ray, ref tmin);
        }
    }
}
