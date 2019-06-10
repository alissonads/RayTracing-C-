using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Mappings
{
    class CylindricalMap : IMapping
    {
        public IMapping Clone()
        {
            return new CylindricalMap();
        }

        public void GetTexelCoordinates(Vec3 localHitPoint, 
                                        int hres, 
                                        int vres, 
                                        ref int row, 
                                        ref int collumn)
        {
            double phi = Math.Atan2(localHitPoint.X, localHitPoint.Z);

            if (phi < 0.0)
                phi += MathUtils.TwoPI;

            double u = phi * MathUtils.Inv2PI;
            double v = (localHitPoint.Y + 1) / 2;

            collumn = (int)((hres - 1) * u);
            row = (int)((vres - 1) * v);
        }
    }
}
