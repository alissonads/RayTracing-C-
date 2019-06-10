using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Tracers.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.World.Base;

namespace RayTracing.Tracers
{
    class PathTrace : Tracer
    {
        public PathTrace(SceneBase w) :
            base(w)
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

                return sr.Material.PathShade(sr);
            }

            return world.BackgroundColor;
        }
    }
}
