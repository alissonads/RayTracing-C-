using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util.Math
{
    class Rnd
    {
        private static Random rand = new Random();

        public static double RandDouble()
        {
            return RandInt()/(double)int.MaxValue;
        }
        public static double RandDouble(double l, double h)
        {
            return RandDouble() * (h - l) + l;
        }
        public static int RandInt()
        {
            return rand.Next(int.MaxValue);
        }
        public static int RandInt(int range)
        {
            return rand.Next(range);
        }
        public static int RandInt(double l, double h)
        {
            return (int)RandDouble(l, h);
        }

        public static void SetRandSeed(int seed)
        {
            rand = new Random(seed);
        }

        public static void Shuffle<T>(List<T> list)
        {
            T []arr = list.ToArray();

            for (int i = list.Count; i > 1; i--)
                Swap(arr, i - 1, rand.Next(i));

            for(int i = 0; i < arr.Length; i++)
            {
                list[i] = arr[i];
            }
        }

        private static void Swap<T>(T []arr, int i, int j)
        {
            T t = arr[i];
            arr[i] = arr[j];
            arr[j] = t;
        }
    }
}
