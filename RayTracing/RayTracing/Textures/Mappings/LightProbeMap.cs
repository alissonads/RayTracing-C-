using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util.Math;

namespace RayTracing.Textures.Mappings
{
    class LightProbeMap : IMapping
    {
        private bool lightProbe;

        public bool LightProbe
        {
            get { return lightProbe; }
            set
            {
                lightProbe = value;
            }

        }

        public LightProbeMap()
        {
            lightProbe = true;
        }

        public LightProbeMap(bool light_probe)
        {
            this.lightProbe = light_probe;
        }

        public LightProbeMap(LightProbeMap other)
        {
            lightProbe = other.lightProbe;
        }

        public void MakePanoramic()
        {
            lightProbe = false;
        }

        public IMapping Clone()
        {
            return new LightProbeMap(this);
        }

        public void GetTexelCoordinates(Vec3 hitPoint, 
                                        int hres, int vres, 
                                        ref int row, 
                                        ref int collumn)
        {
            double x = hitPoint.X;
            double y = hitPoint.Y;
            double z = hitPoint.Z;

            double d = Math.Sqrt(x * x + y * y);
            double sinBeta = y / d;
            double cosBeta = x / d;
            double alpha = (lightProbe) ? Math.Acos(z) : Math.Acos(-z);

            double r = alpha * MathUtils.InvPI;
            double u = (1.0 + r * cosBeta) * 0.5;
            double v = (1.0 + r * sinBeta) * 0.5;
            collumn = (int)((hres - 1) * u);
            row = (int)((vres - 1) * v);
        }
    }
}
