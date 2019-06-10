using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Materials.Base
{
    interface IEmissiveMaterial
    {
        Vec3 GetLe(ShadeRec sr);

        IEmissiveMaterial Clone();
    }
}
