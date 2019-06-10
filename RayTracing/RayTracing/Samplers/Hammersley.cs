using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;

namespace RayTracing.Samplers
{
    class Hammersley : Sampler
    {
        public Hammersley() :
            base()
        {
            Generate();
        }

        public Hammersley(int numSamples) :
            base(numSamples, 83)
        {
            Generate();
        }

        public Hammersley(int numSamples, int numSets) :
            base(numSamples, numSets)
        {}

        public Hammersley(Hammersley other) :
            base(other)
        {}

        public override Sampler Clone()
        {
            return new Hammersley(this);
        }

        protected override void GenerateSamples()
        {
            for(int p = 0; p < numSets; p++) 
                for(int j = 0; j < numSamples; j++)
                {
                    Vec2 pv = new Vec2((double)j / numSamples, Phi(j));
                    samples.Add(pv);
                }
        }

        private double Phi(int j)
        {
            double x = 0.0;
            double f = 0.5;

            while (j != 0)
            {
                x += f * (double)(j % 2);
                j /= 2;
                f *= 0.5;
            }

            return x;
        }
    }
}
