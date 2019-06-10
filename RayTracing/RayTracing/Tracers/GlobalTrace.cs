using RayTracing.World.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Tracers
{
    class GlobalTrace : Base.Tracer
    {
        public GlobalTrace(SceneBase world) :
            base(world)
        {}

        public override Vec3 TraceRay(Ray ray, int depth)
        {
            if (depth > world.ViewPlane.MaxDepth)
                return ColorUtils.BLACK;

            ShadeRec sr = world.HitObjects(ray);

            if (sr.HitAnObject)
            {
                sr.Depth = depth;
                sr.Ray = ray;

                return sr.Material.GlobalShade(sr);
            }

            return world.BackgroundColor;
        }
    }
}
