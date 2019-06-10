using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Part;
using RayTracing.Util;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Composite
{
    class Glass : Compound
    {
        private BBox bbox;

        public override BBox BoundingBox
        {
            get
            {
                return bbox;
            }

            set
            {
                bbox = value;
            }
        }

        public Glass() :
            this(2, 0.9, 1.0)
        {}

        public Glass(double height,
                     double innerRadius,
                     double outerRadius) :
            this(height, innerRadius, 
                 outerRadius, null)
        {}

        public Glass(double height,
                     double innerRadius,
                     double outerRadius,
                     IMaterial material) :
            base()
        {
            Setup(height,
                  innerRadius,
                  outerRadius);
            Material = material;
        }

        public Glass(Glass other) :
            base(other)
        {}

        public override GeometricObject Clone()
        {
            return new Glass(this);
        }

        private void Setup(double height,
                           double innerRadius,
                           double outerRadius)
        {
            AddObject(new Annulus(new Vec3(0, height, 0),
                                  new Vec3(0, 1, 0),
                                  innerRadius,
                                  outerRadius));

            AddObject(new Disk(new Vec3(),
                               new Vec3(0, -1, 0),
                               outerRadius));

            AddObject(new ConcavePartCylinder(0, height, innerRadius));

            AddObject(new ConvexPartCylinder(0, height, outerRadius));
            
            double x0 = -outerRadius;
            double x1 = outerRadius;
            double y0 = 0;
            double y1 = height;
            double z0 = -outerRadius;
            double z1 = outerRadius;

            bbox = new BBox(x0, x1, y0, y1, z0, z1);
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
