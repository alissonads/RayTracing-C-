using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;

namespace RayTracing.Samplers
{
    class Regular : Sampler
    {
        public Regular() :
            base()
        {
            Generate();
        }

        public Regular(int numSamples) :
            base(numSamples, 83)
        {
            Generate();
        }

        public Regular(int numSamples, int numSets) :
            base(numSamples, numSets)
        {}

        public Regular(Regular other) :
            base(other)
        {}

        public override Sampler Clone()
        {
            return new Regular(this);
        }

        protected override void GenerateSamples()
        {
            int n = (int)Math.Sqrt(numSamples);

            for(int p = 0; p < numSets; p++)
                for(int j = 0; j < n; j++)
                    for(int k = 0; k < n; k++)
                    {
                        Vec2 sp = new Vec2((k + 0.5) / n,
                                           (j + 0.5) / n);
                        samples.Add(sp);
                    }
        }
    }
}
