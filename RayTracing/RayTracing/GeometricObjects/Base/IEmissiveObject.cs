using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects.Base
{
    interface IEmissiveObject
    {
        Vec3 Sample();

        Vec3 GetNormal(Vec3 sp);

        float PDF(ShadeRec sr);

        IEmissiveMaterial EmissiveMaterial { get; set; }

        IEmissiveObject CloneEmissive();
    }
}
