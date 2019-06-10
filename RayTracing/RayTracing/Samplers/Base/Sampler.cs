using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;

namespace RayTracing.Samplers.Base
{
    abstract class Sampler
    {
        private List<Vec2> diskSamples;
        private List<Vec3> hemisphereSamples;
        private List<Vec3> sphereSamples;
        private List<int> shuffledIndices;
        private ulong count;
        private int jump;

        protected int numSamples;
        protected int numSets;
        protected List<Vec2> samples;

        public int NumSamples
        {
            get { return numSamples; }
            set { numSamples = value; }
        }

        public int NumSets
        {
            get { return numSets; }
            set { numSets = value; }
        }

        public Sampler()
        {
            numSamples = 1;
            numSets = 83;
            samples = new List<Vec2>(numSamples * numSets);
            diskSamples = new List<Vec2>();
            hemisphereSamples = new List<Vec3>();
            sphereSamples = new List<Vec3>();
            shuffledIndices = new List<int>(numSamples * numSets);
            count = 0;
            jump = 0;
            //Generate();
        }

        public Sampler(int numSamples, int numSets)
        {
            this.numSamples = numSamples;
            this.numSets = numSets;
            samples = new List<Vec2>(numSamples * numSets);
            diskSamples = new List<Vec2>();
            hemisphereSamples = new List<Vec3>();
            sphereSamples = new List<Vec3>();
            shuffledIndices = new List<int>(numSamples * numSets);
            count = 0;
            jump = 0;
            //Generate();
        }

        public Sampler(Sampler other)
        {
            numSamples = other.numSamples;
            numSets = other.numSets;
            samples = new List<Vec2>(other.samples);
            diskSamples = new List<Vec2>(other.diskSamples);
            hemisphereSamples = new List<Vec3>(other.hemisphereSamples);
            sphereSamples = new List<Vec3>(other.sphereSamples);
            shuffledIndices = new List<int>(other.shuffledIndices);
            count = other.count;
            jump = other.jump;
        }
        
        public abstract Sampler Clone();

        protected abstract void GenerateSamples();

        protected void SetupShuffledIndices()
        {
            shuffledIndices.Capacity = numSamples * numSets;
            List<int> indices = new List<int>();

            for (int j = 0; j < numSamples; j++)
                indices.Add(j);

            for(int p = 0; p < numSets; p++)
            {
                Rnd.Shuffle(indices);

                for (int j = 0; j < numSamples; j++)
                    shuffledIndices.Add(indices[j]);
            }
        }

        public void ShuffleSamples()
        {

        }

        public void MapSamplesToUnitDisk()
        {
            int size = samples.Count;
            double r, phi;                          
            Vec2 sp = new Vec2();

            diskSamples.Capacity = size;

            for (int j = 0; j < size; j++)
            {
                sp.X = 2.0 * samples[j].X - 1.0;
                sp.Y = 2.0 * samples[j].Y - 1.0;

                if (sp.X > -sp.Y)
                {
                    if (sp.X > sp.Y)
                    {
                        r = sp.X;
                        phi = sp.Y / sp.X;
                    }
                    else 
                    {
                        r = sp.Y;
                        phi = 2.0 - sp.X / sp.Y;
                    }
                }
                else
                {
                    if (sp.X < sp.Y)
                    {
                        r = -sp.X;
                        phi = 4.0 + sp.Y / sp.X;
                    }
                    else
                    {
                        r = -sp.Y;
                        if (sp.Y != 0.0)
                            phi = 6.0 - sp.X / sp.Y;
                        else
                            phi = 0.0;
                    }
                }

                phi *= MathUtils.PI / 4.0;

                diskSamples.Add(new Vec2(r * Math.Cos(phi), r * Math.Sin(phi)));
            }

            samples.Clear();
        }

        public void MapSamplesToHemisphere(double e)
        {
            int size = samples.Count;
            hemisphereSamples.Capacity = size;

            for (int j = 0; j < size; j++)
            {
                double cosPhi = Math.Cos(2.0 * MathUtils.PI * samples[j].X);
                double sinPhi = Math.Sin(2.0 * MathUtils.PI * samples[j].X);
                double cosTheta = Math.Pow((1.0 - samples[j].Y), 1.0 / (e + 1.0));
                double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);
                double pu = sinTheta * cosPhi;
                double pv = sinTheta * sinPhi;
                double pw = cosTheta;

                hemisphereSamples.Add(new Vec3(pu, pv, pw));
            }
        }

        public void MapSamplesToSphere()
        {
            double r1, r2;
            double x, y, z;
            double r, phi;

            int size = numSamples * numSets;
            sphereSamples.Capacity = size;

            for (int j = 0; j < size; j++)
            {
                r1 = samples[j].X;
                r2 = samples[j].Y;
                z = 1.0 - 2.0 * r1;
                r = Math.Sqrt(1.0 - z * z);
                phi = MathUtils.TwoPI * r2;
                x = r * Math.Cos(phi);
                y = r * Math.Sin(phi);
                sphereSamples.Add(new Vec3(x, y, z));
            }
        }

        public Vec2 SampleUnitSquare()
        {
            if (count % (uint)numSamples == 0)
                jump = (Rnd.RandInt() % numSets) * numSamples;

            return (samples[jump + shuffledIndices[(int)((uint)jump + count++ % (uint)numSamples)]]);
        }

        public Vec2 SampleUnitDisk()
        {
            if (count % (uint)numSamples == 0)
                jump = (Rnd.RandInt() % numSets) * numSamples;

            return (diskSamples[jump + shuffledIndices[(int)((uint)jump + count++ % (uint)numSamples)]]);
        }

        public Vec3 SampleHemisphere()
        {
            if (count % (uint)numSamples == 0)
                jump = (Rnd.RandInt() % numSets) * numSamples;

            return (hemisphereSamples[jump + shuffledIndices[(int)((uint)jump + count++ % (uint)numSamples)]]);
        }

        public Vec3 SampleSphere()
        {
            if (count % (uint)numSamples == 0)
                jump = (Rnd.RandInt() % numSets) * numSamples;

            return (sphereSamples[jump + shuffledIndices[(int)((uint)jump + count++ % (uint)numSamples)]]);
        }

        public void Generate()
        {
            GenerateSamples();
            SetupShuffledIndices();
        }
    }
}
