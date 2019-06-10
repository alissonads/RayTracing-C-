using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.BTDFs.Base
{
    abstract class BTDF
    {
        public BTDF() {}
        
        public abstract BTDF Clone();

        public virtual Vec3 F(ShadeRec sr, Vec3 wt, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 SampleF(ShadeRec sr, ref Vec3 wt, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }

        public virtual Vec3 RHO(ShadeRec sr, Vec3 wo)
        {
            return ColorUtils.BLACK;
        }

        public abstract bool Tir(ShadeRec sr);
    }
}
