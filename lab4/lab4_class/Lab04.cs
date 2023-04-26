using System;
using ASD.Graphs;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ASD
{
    public class Lab04 : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - wyznaczanie numerów grup, które jest w stanie odwiedzić Karol, zapisując się na początku do podanej grupy
        /// </summary>
        /// <param name="graph">Ważony graf skierowany przedstawiający zasady dołączania do grup</param>
        /// <param name="start">Numer grupy, do której początkowo zapisuje się Karol</param>
        /// <returns>Tablica numerów grup, które może odwiedzić Karol, uporządkowana rosnąco</returns>
        public int[] Lab04Stage1(DiGraph<int> graph, int start)
        {
            Stack<int[]> queue = new Stack<int[]>();
            var visited = new bool[graph.VertexCount];
            bool[,] removedEdges = new bool[graph.VertexCount, graph.VertexCount];
            //var result = new List<int>();
            queue.Push(new int[] { start, -1 });

            while (queue.Count > 0)
            {
                int[] current = queue.Pop();
                int currentGroup = current[0];
                int lastGroup = current[1];

                visited[currentGroup] = true;


                foreach (var edge in graph.OutEdges(currentGroup))
                {
                    int targetGroup = edge.To;
                    int edgeWeight = edge.Weight;

                    if (edgeWeight == lastGroup )
                    {
                        removedEdges[currentGroup, targetGroup] = true;
                        queue.Push(new int[] { targetGroup, currentGroup });
                    }
                    else if (edgeWeight == -1 && currentGroup == start && lastGroup == -1)
                    {
                        queue.Push(new int[] { targetGroup, currentGroup });
                    }
                }

            }

            //List<int> result = new List<int>();
            //for (int i = 0; i < visited.Length; i++)
            //{
            //    if (visited[i])
            //    {
            //        result.Add(i);
            //    }
            //}

            return Enumerable.Range(0, visited.Length)
                 .Where(i => visited[i])
                 .ToArray();


        }






        /// <summary>
        /// Etap 2 - szukanie możliwości przejścia z jednej z grup z `starts` do jednej z grup z `goals`
        /// </summary>
        /// <param name="graph">Ważony graf skierowany przedstawiający zasady dołączania do grup</param>
        /// <param name="starts">Tablica z numerami grup startowych (trasę należy zacząć w jednej z nich)</param>
        /// <param name="goals">Tablica z numerami grup docelowych (trasę należy zakończyć w jednej z nich)</param>
        /// <returns>(possible, route) - `possible` ma wartość true gdy istnieje możliwość przejścia, wpp. false, 
        /// route to tablica z numerami kolejno odwiedzanych grup (pierwszy numer to numer grupy startowej, ostatni to numer grupy docelowej),
        /// jeżeli possible == false to route ustawiamy na null</returns>
        public (bool possible, int[] route) Lab04Stage2(DiGraph<int> graph, int[] starts, int[] goals)
        {

            int n = graph.VertexCount;
            DiGraph<int> help = new DiGraph<int>(n * n, graph.Representation);

            foreach (var edge in graph.DFS().SearchAll())
            {
                var u = edge.From;
                var v = edge.To;
                var w = edge.Weight;
                if (w == u)
                {
                    continue;
                }
                // case with the weight is < 0
                if (w == -1)
                {
                    help.AddEdge(u * n + u, v * n + u);
                }
                else
                {
                    help.AddEdge(u * n + w, v * n + u);
                }
            }

            List<int> route = new List<int>();
            //HashSet<int> visited = new HashSet<int>();

            foreach (int start in starts)
            {
                foreach (int goal in goals)
                {
                    route.Clear();
                    //visited.Add(current);
                    //dfs na help ze start 
                    // w tym dfs sprawdzic czy kiedy

                    foreach (var edge in help.DFS().SearchFrom(start * n + start))
                    {
                        if (edge.To / n == goal)
                        {


                            int to = edge.To / n;
                            int from = edge.From / n;
                            route.Add(to);

                            while (from != start || graph.GetEdgeWeight(from, to) != -1)
                            {
                                route.Add(from);
                                int weigh = graph.GetEdgeWeight(from, to);
                                to = from;
                                from = weigh;

                            }
                            route.Add(start);
                            route.Reverse();
                            return (true, route.ToArray());



                        }


                    }
                    if (start == goal)
                    {
                        route.Add(start);
                        return (true, route.ToArray());
                    }
                }

            }
            return (false, null);

        }

        private int[] BuildRoute(int[] starts, int[] goals, bool[,] removedEdges, int n)
{
    int start = starts[0];
    int goal = goals[0];
    int[] route = new int[2] { start, goal };
    int current = start;

    while (current != goal)
    {
        bool foundNext = false;
        for (int i = 0; i < n; i++)
        {
            if (current != i && !removedEdges[current, i] && !removedEdges[i, current])
            {
                route = route.Append(i).ToArray();
                current = i;
                foundNext = true;
                break;
            }
        }

        if (!foundNext)
            return null;
    }

    return route;
}
        public static int[] ShortestPath(DiGraph<int> graph, int[] starts, int[] goals)
        {
            int[] distances = Enumerable.Repeat(int.MaxValue, graph.VertexCount).ToArray();
            bool[] visited = new bool[graph.VertexCount];
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>(Comparer<int>.Default);

            foreach (int start in starts)
            {
                distances[start] = 0;
                queue.Insert(start, 0);
            }

            while (queue.Count > 0)
            {
                int currVertex = queue.Extract();

                if (visited[currVertex])
                {
                    continue;
                }

                visited[currVertex] = true;

                foreach (var neighbor in graph.OutEdges(currVertex))
                {
                    int neighborVertex = neighbor.To;

                    if (visited[neighborVertex])
                    {
                        continue;
                    }

                    int newDistance = distances[currVertex] + neighbor.Weight;

                    if (newDistance < distances[neighborVertex])
                    {
                        distances[neighborVertex] = newDistance;
                        queue.Insert(neighborVertex, newDistance);
                    }
                }
            }

            return goals.Select(goal => distances[goal]).ToArray();

        }
    }
}
