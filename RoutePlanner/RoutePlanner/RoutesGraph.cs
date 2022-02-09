using System;

namespace RoutePlanner
{
    public class RoutesGraph
    {
        private double[,] Graph;
        private int Size = 0;

        public RoutesGraph()
        {
            string[] lines = Properties.Resources.Routes.Split('\n');

            Size = lines.Length;
            Graph = new double[Size, Size];

            for (int i = 0; i < Size; ++i)
            {
                string[] numbers = lines[i].Split(' ');
                for (int j = 0; j < Size; ++j)
                    Graph[i, j] = Convert.ToDouble(numbers[j]);
            }

        }

        public double this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < Size && j >= 0 && j < Size)
                    return Graph[i, j];
                throw new IndexOutOfRangeException();
            }
        }
    }
}
