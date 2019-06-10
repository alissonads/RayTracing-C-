using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.GeometricObjects.Part;
using RayTracing.Util.Math;
using RayTracing.GeometricObjects.Primitives;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Composite
{
    class Bowl : Compound
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

        private Bowl() :
            base()
        {}
        
        private Bowl(Bowl other) :
            base(other)
        {
            if(other.bbox != null)
                bbox = other.bbox.Clone();
        }

        public override GeometricObject Clone()
        {
            return new Bowl(this);
        }

        public static Bowl CreateFlatRimmedBowl(double innerRadius,
                                                double outerRadius)
        {
            return CreateFlatRimmedBowl(innerRadius, outerRadius, null);
        }

        public static Bowl CreateFlatRimmedBowl(double innerRadius,
                                                double outerRadius,
                                                IMaterial material)
        {
            ConvexPartSphere o = new ConvexPartSphere(new Vec3(),
                                                      outerRadius,
                                                      0, 360, 90, 180);
            ConvexPartSphere i = new ConvexPartSphere(new Vec3(),
                                                      innerRadius,
                                                      0, 360, 90, 180);
            Annulus a = new Annulus(new Vec3(),
                                    new Vec3(0, 1, 0),
                                    innerRadius,
                                    outerRadius);

            Bowl b = new Bowl();
            b.AddObject(o);
            b.AddObject(i);
            b.AddObject(a);

            b.bbox = new BBox(-outerRadius, outerRadius,
                              -outerRadius, outerRadius,
                              -outerRadius, outerRadius);

            b.Material = material;

            return b;
        }

        public static Bowl CreateRoundRimmedBowl(double innerRadius,
                                                 double outerRadius)
        {
            return CreateRoundRimmedBowl(innerRadius, outerRadius, null);
        }

        public static Bowl CreateRoundRimmedBowl(double innerRadius,
                                                 double outerRadius,
                                                 IMaterial material)
        {
            ConvexPartSphere o = new ConvexPartSphere(new Vec3(),
                                                      outerRadius,
                                                      0, 360, 90, 180);
            ConvexPartSphere i = new ConvexPartSphere(new Vec3(),
                                                      innerRadius,
                                                      0, 360, 90, 180);
            Torus t = new Torus((outerRadius + innerRadius) / 2,
                                (outerRadius - innerRadius) / 2);

            Bowl b = new Bowl();
            b.AddObject(o);
            b.AddObject(i);
            b.AddObject(t);

            b.bbox = new BBox(-outerRadius, outerRadius,
                              -outerRadius, outerRadius,
                              -outerRadius, outerRadius);

            b.Material = material;

            return b;
        }

        public static Bowl Create(double innerRadius,
                                  double outerRadius,
                                  bool roundRimmedBowl)
        {
            return Create(innerRadius, outerRadius, roundRimmedBowl, null);
        }

        public static Bowl Create(double innerRadius,
                                  double outerRadius,
                                  bool roundRimmedBowl,
                                  IMaterial material)
        {
            ConvexPartSphere o = new ConvexPartSphere(new Vec3(),
                                                      outerRadius,
                                                      0, 360, 90, 180);
            ConvexPartSphere i = new ConvexPartSphere(new Vec3(),
                                                      innerRadius,
                                                      0, 360, 90, 180);

            Bowl b = new Bowl();
            b.AddObject(o);
            b.AddObject(i);

            if (roundRimmedBowl)
            {
                Torus t = new Torus((outerRadius + innerRadius) / 2,
                                    (outerRadius - innerRadius) / 2);
                b.AddObject(t);
            }
            else
            {
                Annulus a = new Annulus(new Vec3(),
                                        new Vec3(0, 1, 0),
                                        innerRadius,
                                        outerRadius);
                b.AddObject(a);
            }

            b.bbox = new BBox(-outerRadius, outerRadius,
                              -outerRadius, outerRadius,
                              -outerRadius, outerRadius);

            b.Material = material;

            return b;
        }

        public void SetExternalMaterial(IMaterial material)
        {
            objects[0].Material = material;
        }

        public void SetInternalMaterial(IMaterial material)
        {
            objects[1].Material = material;
        }

        public void SetBorderMaterial(IMaterial material)
        {
            objects[2].Material = material;
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
