using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Mappings
{
    class RectangularMap : Base.IMapping
    {
        public IMapping Clone()
        {
            return new RectangularMap();
        }

        public void GetTexelCoordinates(Vec3 localHitPoint, int hres, int vres, ref int row, ref int collumn)
        {
            double u = (localHitPoint.Z + 1) / 2;
            double v = (localHitPoint.X + 1) / 2;

            collumn = (int)((hres - 1) * u);
            row = (int)((vres - 1) * v);
        }
    }
}
