using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Textures.Base
{
    interface IMapping
    {
        IMapping Clone();

        void GetTexelCoordinates(Vec3 localHitPoint,
                                 int hres, int vres,
                                 ref int row, 
                                 ref int collumn);
    }
}
