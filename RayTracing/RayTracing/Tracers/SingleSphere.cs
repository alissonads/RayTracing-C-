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
    class SingleSphere : Tracer
    {
        
        public SingleSphere(SceneBase scene) :
            base(scene)
        {}
        
        public override Vec3 TraceRay(Ray ray)
        {
            ShadeRec sr = new ShadeRec(world.HitBareBonesObjects(ray));

            if (sr.HitAnObject)
                return sr.Color;

            return base.TraceRay(ray);
        }
    }
}
