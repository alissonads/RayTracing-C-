using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Materials.Base
{
    interface IMaterial
    {
        IMaterial Clone();

        Vec3 Shade(ShadeRec sr);

        Vec3 AreaLightShade(ShadeRec sr);

        Vec3 PathShade(ShadeRec sr);

        Vec3 GlobalShade(ShadeRec sr);
    }
}
