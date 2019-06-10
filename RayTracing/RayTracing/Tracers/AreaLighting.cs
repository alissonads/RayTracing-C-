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
    class AreaLighting : Tracer
    {
        public AreaLighting(SceneBase world) :
            base(world)
        {}

        public override Vec3 TraceRay(Ray ray, int depth)
        {
            double tmin = 0;
            return TraceRay(ray, ref tmin, depth);
        }

        public override Vec3 TraceRay(Ray ray, ref double tmin, int depth)
        {
            if (depth > world.ViewPlane.MaxDepth)
                return ColorUtils.BLACK;
            else
            {
                ShadeRec sr = world.HitObjects(ray);

                if (sr.HitAnObject)
                {
                    sr.Ray = ray;
                    sr.Depth = depth;
                    tmin = sr.Tmin;

                    return sr.Material.AreaLightShade(sr);
                }

                tmin = MathUtils.HugeValue;

                return world.BackgroundColor;
            }
        }
    }
}
