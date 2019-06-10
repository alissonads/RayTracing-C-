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
    class GlassWithLiquid : Compound
    {
        private BBox bbox;

        public GlassWithLiquid(double height,
                               double innerRadius,
                               double wallThickness,
                               double baseThickness,
                               double waterHeight,
                               double meniscusRadius) :
            base()
        {
            Setup(height,
                  innerRadius,
                  wallThickness,
                  baseThickness,
                  waterHeight,
                  meniscusRadius);
        }

        public GlassWithLiquid(double glassHeight,
                               double liquidHeight,
                               double innerRadius,
                               double outerRadius) :
            base()
        {
            Setup(glassHeight,
                  liquidHeight,
                  innerRadius,
                  outerRadius);
        }

        private void Setup(double height,
                           double innerRadius,
                           double wallThickness,
                           double baseThickness,
                           double waterHeight,
                           double meniscusRadius)
        {
            AddObject(new Annulus(new Vec3(0, height, 0),
                                  new Vec3(0, 1, 0),
                                  innerRadius,
                                  innerRadius + wallThickness));

            AddObject(new Disk(new Vec3(),
                               new Vec3(0, -1, 0),
                               innerRadius + wallThickness));

            AddObject(new ConcavePartCylinder(waterHeight + meniscusRadius,
                                              height, innerRadius));

            AddObject(new ConvexPartCylinder(0, height, innerRadius + wallThickness));

            //liquid
            AddObject(new Disk(new Vec3(0, waterHeight, 0),
                               new Vec3(0, 1, 0),
                               innerRadius - meniscusRadius));

            AddObject(new Disk(new Vec3(0, baseThickness, 0),
                               new Vec3(0, -1, 0),
                               innerRadius));

            AddObject(new ConvexPartCylinder(baseThickness,
                                             waterHeight + meniscusRadius,
                                             innerRadius));

            double x0 = -(innerRadius + wallThickness);
            double x1 = innerRadius + wallThickness;
            double y0 = 0;
            double y1 = height;
            double z0 = -(innerRadius + wallThickness);
            double z1 = innerRadius + wallThickness;

            bbox = new BBox(x0, x1, y0, y1, z0, z1);
        }

        private void Setup(double glassHeight,
                           double liquidHeight,
                           double innerRadius,
                           double outerRadius)
        {
            AddObject(new Glass(glassHeight, innerRadius, outerRadius));
            //liquid
            AddObject(new Disk(new Vec3(0, liquidHeight, 0),
                               new Vec3(0, 1, 0),
                               innerRadius - 0.1));
            AddObject(new ConvexPartCylinder(0.3, liquidHeight+0.1, innerRadius - 0.001));
            AddObject(new Disk(new Vec3(0, 0.3, 0),
                               new Vec3(0, -1, 0),
                               innerRadius - 0.1));

            double x0 = -outerRadius;
            double x1 = outerRadius;
            double y0 = 0;
            double y1 = glassHeight;
            double z0 = -outerRadius;
            double z1 = outerRadius;

            bbox = new BBox(x0, x1, y0, y1, z0, z1);
        }

        public void SetGlassAirMaterial(IMaterial m)
        {
            //for (int j = 0; j < 4; j++)
            //    objects[j].Material = m;
            objects[0].Material = m;
        }

        public void SetLiquidAirMaterial(IMaterial m)
        {
            objects[1].Material = m;
            
            //objects[4].Material = m;
        }

        public void SetLiquidGlassMaterial(IMaterial m)
        {
            //objects[5].Material = m;
            //objects[6].Material = m;
            objects[2].Material = m;
            objects[3].Material = m;
        }

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
