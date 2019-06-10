using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;

namespace RayTracing.Samplers
{
    class Jittered : Sampler
    {
        public Jittered() :
            base()
        {
            Generate();
        }

        public Jittered(int numSamples) :
            base(numSamples, 83)
        {
            Generate();
        }

        public Jittered(int numSamples, int numSets) :
            base(numSamples, numSets)
        {}

        public Jittered(Jittered other) :
            base(other)
        {}

        public override Sampler Clone()
        {
            return new Jittered(this);
        }

        protected override void GenerateSamples()
        {
            int n = (int)Math.Sqrt(numSamples);

            for (int p = 0; p < numSets; p++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < n; k++)
                    {
                        Vec2 sp = new Vec2((k + Rnd.RandDouble()) / n,
                                           (j + Rnd.RandDouble()) / n);
                        samples.Add(sp);
                    }
        }

    }
}
