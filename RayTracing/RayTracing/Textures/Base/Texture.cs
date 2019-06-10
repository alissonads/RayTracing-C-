using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Textures.Base
{
    interface ITexture
    {
        Vec3 GetColor(ShadeRec sr);

        ITexture Clone();
    }
}
