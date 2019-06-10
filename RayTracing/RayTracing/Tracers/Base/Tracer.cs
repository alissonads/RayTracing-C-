using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.Util.Math;
using RayTracing.Util;

namespace RayTracing.Tracers.Base
{
    abstract class Tracer
    {
        protected SceneBase world;
        
        public Tracer(SceneBase world)
        {
            this.world = world;
        }

        public virtual Vec3 TraceRay(Ray ray)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 TraceRay(Ray ray, int depth)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 TraceRay(Ray ray, ref double tmin, int depth)
        {
            return ColorUtils.BLACK;
        }
    }
}
