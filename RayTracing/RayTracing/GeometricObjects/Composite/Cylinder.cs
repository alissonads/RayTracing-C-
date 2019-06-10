using RayTracing.GeometricObjects.Primitives;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;

namespace RayTracing.GeometricObjects.Composite
{
    class Cylinder : Compound
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

        private Cylinder() :
            base()
        {}
        
        private Cylinder(Cylinder other) :
            base(other)
        {
            if (other.bbox != null)
                bbox = other.bbox;
        }

        public override GeometricObject Clone()
        {
            return new Cylinder(this);
        }

        //bevelRadius > 0 return beveled cylinder
        //bevelRadius <=0 return solid cylinder
        public static Cylinder Create(float bottom,
                                      float top,
                                      float radius,
                                      float bevelRadius)
        {
            return Create(bottom, top, radius, bevelRadius, null);
        }

        //bevelRadius > 0 return beveled cylinder
        //bevelRadius <=0 return solid cylinder
        public static Cylinder Create(float bottom,
                                      float top,
                                      float radius,
                                      float bevelRadius,
                                      IMaterial material)
        {
            Cylinder c = new Cylinder();

            c.AddObject(new Disk(new Vec3(0, top, 0),
                               new Vec3(0, 1, 0),
                               radius - bevelRadius));

            c.AddObject(new OpenCylinder(bottom + bevelRadius,
                                       top - bevelRadius,
                                       radius));

            c.AddObject(new Disk(new Vec3(0, bottom, 0),
                                 new Vec3(0, -1, 0),
                                 radius - bevelRadius));

            if (bevelRadius > 0)
            {
                Instance topBevel = new Instance(new Torus(radius - bevelRadius,
                                                           bevelRadius));
                topBevel.Translate(0, top - bevelRadius, 0);
                c.AddObject(topBevel);

                Instance bottomBevel = new Instance(new Torus(radius - bevelRadius,
                                                              bevelRadius));
                bottomBevel.Translate(0, bottom + bevelRadius, 0);
                c.AddObject(bottomBevel);
            }

            c.bbox = new BBox(-radius, radius, bottom, top, -radius, radius);
            c.Material = material;

            return c;
        }

        //public void SetTopMaterial(Material material)
        //{
        //    objects[0].Material = material;
        //}

        //public void SeTopBorderMaterial(Material material)
        //{
        //    objects[1].Material = material;
        //}

        //public void SetWallMaterial(Material material)
        //{
        //    objects[1].Material = material;
        //}

        //public void SetBottomBorderMaterial(Material material)
        //{
        //    objects[2].Material = material;
        //}

        //public void SetBottomMaterial(Material material)
        //{
        //    objects[2].Material = material;
        //}
        
        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            if (bbox.Hit(ray))
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
