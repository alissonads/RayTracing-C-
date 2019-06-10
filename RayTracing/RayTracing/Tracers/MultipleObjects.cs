using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Tracers.Base;

namespace RayTracing.Tracers
{
    using Util;
    using Util.Math;
    using World.Base;

    class MultipleObjects : Tracer
    {
        public MultipleObjects(SceneBase world) : 
            base(world)
        {}

        public override Vec3 TraceRay(Ray ray)
        {
            ShadeRec sr = world.HitBareBonesObjects(ray);

            if (sr.HitAnObject)
                return sr.Color;

            return world.BackgroundColor;
        }
    }
}
