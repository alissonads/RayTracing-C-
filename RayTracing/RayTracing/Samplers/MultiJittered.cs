using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Samplers.Base;
using RayTracing.Util.Math;

namespace RayTracing.Samplers
{
    class MultiJittered : Sampler
    {
        public MultiJittered() :
            base()
        {
            Generate();
        }

        public MultiJittered(int numSamples) :
            base(numSamples, 83)
        {
            Generate();
        }

        public MultiJittered(int numSamples, int numSets) :
            base(numSamples, numSets)
        {}

        public MultiJittered(MultiJittered other) :
            base(other)
        {}

        public override Sampler Clone()
        {
            return new MultiJittered(this);
        }

        protected override void GenerateSamples()
        {
            int n = (int)Math.Sqrt(numSamples);
            double subcellWidth = 1.0 / ((double)numSamples);
            int s = numSamples * numSets;
            int id = 0;

            for (int i = 0; i < s; i++)
                samples.Add(new Vec2());

            for (int p = 0; p < numSets; p++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        id = i * n + j + p * numSamples;
                        samples[id].X = ((i * n + j) * subcellWidth + Rnd.RandDouble(0.0, subcellWidth));
                        samples[id].Y = ((j * n + i) * subcellWidth + Rnd.RandDouble(0.0, subcellWidth));
                    }

            ShuffledXCoordinates(n);
            ShuffledYCoordinates(n);
        }

        void ShuffledXCoordinates(int n)
        {
	        for (int p = 0; p < numSets; p++)
		        for (int i = 0; i < n; i++)
			        for (int j = 0; j < n; j++)
			        {
				        int k = Rnd.RandInt(j, (n - 1));
                        double t = samples[i * n + j + p * numSamples].X;
                        samples[i * n + j + p * numSamples].X = samples[i * n + k + p * numSamples].X;
				        samples[i * n + k + p * numSamples].X = t;
			        }
        }

        void ShuffledYCoordinates(int n)
        {
            for (int p = 0; p < numSets; p++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        int k = Rnd.RandInt(j, (n - 1));
                        double t = samples[i * n + j + p * numSamples].Y;
                        samples[i * n + j + p * numSamples].Y = samples[i * n + k + p * numSamples].Y;
                        samples[i * n + k + p * numSamples].Y = t;
                    }
        }
    }
}
