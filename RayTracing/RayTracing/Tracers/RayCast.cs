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
    class RayCast : Tracer
    {
        public RayCast(SceneBase world) :
            base(world)
        {}

        public override Vec3 TraceRay(Ray ray, int depth)
        {
            ShadeRec sr = world.HitObjects(ray);

            if (sr.HitAnObject)
            {
                sr.Ray = ray;
                return sr.Material.Shade(sr);
            }

            return world.BackgroundColor;
        }
    }
}
