using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;

namespace RayTracing.Samplers
{
    class NRooks : Sampler
    {
        public NRooks() :
            base()
        {
            Generate();
        }

        public NRooks(int numSamples) :
            base(numSamples, 83)
        {
            Generate();
        }

        public NRooks(int numSamples, int numSets) :
            base(numSamples, numSets)
        {}

        public NRooks(NRooks other) :
            base(other)
        {}

        public override Sampler Clone()
        {
            return new NRooks(this);
        }

        protected override void GenerateSamples()
        {
            for (int p = 0; p < numSets; p++)
                for (int j = 0; j < numSamples; j++)
                {
                    Vec2 pv = new Vec2((j + Rnd.RandDouble()) / numSamples,
                                       (j + Rnd.RandDouble()) / numSamples);
                    samples.Add(pv);
                }
        }

        private void ShuffledXCoordinates()
        {
            for (int p = 0; p < numSets; p++)
                for (int i = 0; i < numSamples - 1; i++)
                {
                    int target = Rnd.RandInt() % numSamples + p * numSamples;
                    double temp = samples[i + p * numSamples + 1].X;
                    samples[i + p * numSamples + 1].X = samples[target].X;
                    samples[target].X = temp;
                }
        }

        private void ShuffledYCoordinates()
        {
            for (int p = 0; p < numSets; p++)
                for (int i = 0; i < numSamples - 1; i++)
                {
                    int target = Rnd.RandInt() % numSamples + p * numSamples;
                    double temp = samples[i + p * numSamples + 1].Y;
                    samples[i + p * numSamples + 1].Y = samples[target].Y;
                    samples[target].Y = temp;
                }
        }
    }
}
