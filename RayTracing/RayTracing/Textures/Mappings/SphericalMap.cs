using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Mappings
{
    class SphericalMap : IMapping
    {
        public SphericalMap() { }
        
        public IMapping Clone()
        {
            return new SphericalMap();
        }

        public void GetTexelCoordinates(Vec3 localHitPoint, 
                                        int hres, int vres, 
                                        ref int row, 
                                        ref int collumn)
        {
            double theta = Math.Acos(localHitPoint.Y);
            double phi = Math.Atan2(localHitPoint.X, localHitPoint.Z);

            if (phi < 0)
                phi += MathUtils.TwoPI;

            double u = phi * MathUtils.Inv2PI;
            double v = 1 - theta * MathUtils.InvPI;

            collumn = (int)((hres - 1) * u);
            row = (int)((vres - 1) * v);
        }
    }
}
