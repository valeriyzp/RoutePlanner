using System.Collections.Generic;
using System.Linq;

namespace RoutePlanner
{
    public class CityMap
    {
        public int Size { get; set; }
        public CitiesDictionary Cities { get; set; }
        public RoutesGraph Graph { get; set; }

        public CityMap()
        {
            Cities = new CitiesDictionary();
            Size = Cities.Count;
            Graph = new RoutesGraph();
        }

        public double[] FindAllDistances(string cityUkrStart, out int[] FindAllRoutesWay)
        {
            int startVertex = Cities.GetIndexUkr(cityUkrStart);

            double[] distances = new double[Size];
            bool[] isInShortestPathTree = new bool[Size];

            for (int i = 0; i < Size; ++i)
            {
                distances[i] = int.MaxValue;
                isInShortestPathTree[i] = false;
            }

            FindAllRoutesWay = new int[Size];
            FindAllRoutesWay[startVertex] = -1;

            distances[startVertex] = 0;

            for (int count = 0; count < Size - 1; ++count)
            {
                int u = MinDistance(distances, isInShortestPathTree);

                isInShortestPathTree[u] = true;

                for (int v = 0; v < Size; ++v)
                {
                    if (!isInShortestPathTree[v] &&
                        Graph[u, v] != 0 &&
                        distances[u] != double.MaxValue &&
                        distances[u] + Graph[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + Graph[u, v];
                        FindAllRoutesWay[v] = u;
                    }
                }
            }

            return distances;
        }

        public Route RouteToNextVertex(int size, int[] previous, double[] distances, int end)
        {
            int[] way = new int[size];
            int count = 1;
            way[0] = end;

            for (int i = end; i >= 0; count++)
            {
                way[count] = previous[i];
                i = previous[i];
            }

            Route route = new Route();

            for (int i = count - 3; i > 0; i--)
            {
                route.Add(new Chain(Cities[way[i]], distances[way[i]], false));
            }

            route.Add(new Chain(Cities[end], distances[end], true));

            return route;
        }

        public Route BuildRoute(string cityUkrStart, List<string> destinationPoints)
        {
            int startVertex = Cities.GetIndexUkr(cityUkrStart);
            List<int> destinationVertex = new List<int>();
            for (int i = 0; i < destinationPoints.Count; ++i)
                destinationVertex.Add(Cities.GetIndexUkr(destinationPoints[i]));

            double minDistance = double.MaxValue;
            int minDistanceIndex = -1;
            int[] previous;
            double[] distances = FindAllDistances(cityUkrStart, out previous);

            foreach (int dstIndex in destinationVertex)
            {
                if (distances[dstIndex] < minDistance)
                {
                    minDistance = distances[dstIndex];
                    minDistanceIndex = dstIndex;
                }
            }

            Route route = new Route();
            route.Add(new Chain(Cities[startVertex], distances[startVertex], true));
            route.AddRange(RouteToNextVertex(Size, previous, distances, minDistanceIndex));

            destinationVertex.Remove(minDistanceIndex);

            while (destinationVertex.Count > 0)
            {
                int prevIndex = minDistanceIndex;
                minDistance = double.MaxValue;

                distances = FindAllDistances(Cities[minDistanceIndex].Ukr, out previous);
                foreach (int dstIndex in destinationVertex)
                {
                    if (distances[dstIndex] < minDistance)
                    {
                        minDistance = distances[dstIndex];
                        minDistanceIndex = dstIndex;
                    }
                }

                Route newPart = RouteToNextVertex(Size, previous, distances, minDistanceIndex);
                foreach (Chain chain in newPart)
                {
                    chain.Length += route.Last().Length;
                }
                route.AddRange(newPart);

                destinationVertex.Remove(minDistanceIndex);
            }

            return route;
        }

        private int MinDistance(double[] dist, bool[] isInShortestPathTree)
        {
            double min = double.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < Size; ++v)
            {
                if (isInShortestPathTree[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public List<string> GetRoutesByDistances(double[] distances)
        {
            List<string> routes = new List<string>();

            for (int i = 0; i < Size; ++i)
                if (distances[i] != 0)
                    routes.Add($"Маршрут до {Cities[i].Ukr}: {distances[i]} км");

            return routes;
        }
    }
}
