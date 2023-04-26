using System;
using ASD.Graphs;
using ASD;
using System.Collections.Generic;

namespace ASD
{

    public class Lab04 : System.MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - szukanie trasy z miasta start_v do miasta end_v, startując w dniu day
        /// </summary>
        /// <param name="g">Ważony graf skierowany będący mapą</param>
        /// <param name="start_v">Indeks wierzchołka odpowiadającego miastu startowemu</param>
        /// <param name="end_v">Indeks wierzchołka odpowiadającego miastu docelowemu</param>
        /// <param name="day">Dzień startu (w tym dniu należy wyruszyć z miasta startowego)</param>
        /// <param name="days_number">Liczba dni uwzględnionych w rozkładzie (tzn. wagi krawędzi są z przedziału [0, days_number-1])</param>
        /// <returns>(result, route) - result ma wartość true gdy podróż jest możliwa, wpp. false, 
        /// route to tablica z indeksami kolejno odwiedzanych miast (pierwszy indeks to indeks miasta startowego, ostatni to indeks miasta docelowego),
        /// jeżeli result == false to route ustawiamy na null</returns>
        public (bool result, int[] route) Lab04_FindRoute(DiGraph<int> g, int start_v, int end_v, int day, int days_number)
        {
            // int d = day;
            //int s = start_v;
            int[] route = null;
            int[] path = new int[(g.VertexCount + 2) * days_number];
            for (int i = 0; i < g.VertexCount * days_number; i++)
                path[i] = -1;
            int n = g.VertexCount;
            int m = g.EdgeCount;
            List<int> list = new List<int>();
            DiGraph gg = new DiGraph(days_number * n, g.Representation);

            foreach (var e in g.DFS().SearchFrom(start_v))
            {
                
                gg.AddEdge(e.From * days_number + ((e.Weight - 1 + days_number) % days_number), e.To * days_number + e.Weight%days_number);
                //path[e.To * days_number + e.Weight] = e.From * days_number + ((e.Weight - 1 + days_number) % days_number);
            }
            Stack<int> S = new Stack<int>();
            S.Push(start_v * days_number + ((days_number + day - 1) % days_number));
            foreach (var e in gg.DFS().SearchFrom(start_v * days_number + ((days_number + day - 1) % days_number)))
            {
                path[e.To] = e.From;
                while (e.From != S.Peek())
                {
                    S.Pop();
                }
                S.Push(e.To);

                if (e.To >= end_v * days_number && e.To < (end_v + 1) * days_number)
                {

                    int v = e.To;
                    while (v!= start_v * days_number + ((days_number + day - 1) % days_number))
                    {
                        v = S.Pop();
                        list.Insert(0,v / days_number);
                    }
                   
                    return (true, list.ToArray());
                }
            }
            return (false, null);


        }

        /// <summary>
        /// Etap 2 - szukanie trasy z jednego z miast z tablicy start_v do jednego z miast z tablicy end_v (startować można w dowolnym dniu)
        /// </summary>
        /// <param name="g">Ważony graf skierowany będący mapą</param>
        /// <param name="start_v">Tablica z indeksami wierzchołków startowych (trasę trzeba zacząć w jednym z nich)</param>
        /// <param name="end_v">Tablica z indeksami wierzchołków docelowych (trasę trzeba zakończyć w jednym z nich)</param>
        /// <param name="days_number">Liczba dni uwzględnionych w rozkładzie (tzn. wagi krawędzi są z przedziału [0, days_number-1])</param>
        /// <returns>(result, route) - result ma wartość true gdy podróż jest możliwa, wpp. false, 
        /// route to tablica z indeksami kolejno odwiedzanych miast (pierwszy indeks to indeks miasta startowego, ostatni to indeks miasta docelowego),
        /// jeżeli result == false to route ustawiamy na null</returns>
        public (bool result, int[] route) Lab04_FindRouteSets(DiGraph<int> g, int[] start_v, int[] end_v, int days_number)
        {
            int n = g.VertexCount;
            DiGraph gg = new DiGraph(n * days_number+2, g.Representation);
            for(int i=0;i<days_number;i++)
            {
                for(int j=0;j<start_v.Length;j++)
                {
                    gg.AddEdge(n * days_number, start_v[j]*days_number+i);
                }
                for (int j = 0; j < end_v.Length; j++)
                {
                    gg.AddEdge(end_v[j]*days_number + ((i + days_number - 1)%days_number), n * days_number + 1);
                }
            }
            foreach (var e in g.DFS().SearchAll())
            {
                gg.AddEdge(e.From * days_number + ((e.Weight - 1 + days_number) % days_number), e.To * days_number + e.Weight);
                //path[e.To * days_number + e.Weight] = e.From * days_number + ((e.Weight - 1 + days_number) % days_number);
            }
            Stack<int> S = new Stack<int>();
            S.Push(n * days_number);
            List<int> list=new List<int>();
            foreach (var e in gg.DFS().SearchFrom(n*days_number))
            {
                while (e.From != S.Peek())
                {
                    S.Pop();
                }
                if (e.To==n*days_number+1)
                {

                    int v = S.Pop();
                    while (v!=n*days_number)
                    {
                        
                        list.Insert(0, v / days_number);
                        v = S.Pop();
                    }

                    return (true, list.ToArray());
                }
                S.Push(e.To);

            }

            return (false, null);
        }
    }
}

//---------------------- Working code but tests are fucked up
using System;
using ASD.Graphs;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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
            List<int> visitedGroups = new List<int>();
            HashSet<int> visited = new HashSet<int>();
            Queue<int> queue = new Queue<int>();

            visitedGroups.Add(start);
            visited.Add(start);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int group = queue.Dequeue();

                foreach (var edge in graph.OutEdges(group))
                {
                    int nextGroup = edge.To;
                    int weight = edge.Weight;

                    if (weight == -1 && visited.Add(nextGroup))
                    {
                        visitedGroups.Add(nextGroup);
                        queue.Enqueue(nextGroup);
                    }
                    else if (visited.Contains(weight) && visited.Add(nextGroup))
                    {
                        visitedGroups.Add(nextGroup);
                        queue.Enqueue(nextGroup);
                    }
                }
            }

            visitedGroups.Sort();
            return visitedGroups.ToArray();

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
            // Create a new graph with reversed edges
            DiGraph<int> reversedGraph = Reverse(graph);

            // Create a set of starting nodes
            HashSet<int> startNodes = new HashSet<int>(starts);

            // Create a set of goal nodes
            HashSet<int> goalNodes = new HashSet<int>(goals);

            // Perform a breadth-first search from the starting nodes
            Queue<int> queue = new Queue<int>(startNodes);
            Dictionary<int, int> parent = new Dictionary<int, int>();
            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                if (goalNodes.Contains(current))
                {
                    // Reconstruct the path from the starting node to the goal node
                    List<int> path = new List<int>();
                    path.Add(current);
                    while (parent.ContainsKey(current))
                    {
                        current = parent[current];
                        path.Add(current);
                        //Console.Write($"{current} --> ");
                    }
                    path.Reverse();
                    
                    return (true, path.ToArray());
                }
                foreach (int neighbor in reversedGraph.OutNeighbors(current))
                {
                    if (!startNodes.Contains(neighbor) || parent.ContainsKey(neighbor))
                    {
                        // Skip nodes that are not part of the starting set or have already been visited
                        continue;
                    }
                    parent[neighbor] = current;
                    
                    queue.Enqueue(neighbor);
                    Console.Write($"{neighbor} --> ");
                }
            }

            // No path found
            return (false, null);
        }
        public static DiGraph<int> Reverse(DiGraph graph)
        {
            var reversed = new DiGraph<int>(graph.VertexCount, graph.Representation);

            // Add edges in the opposite direction
            for (int v = 0; v < graph.VertexCount; v++)
            {
                foreach (int w in graph.OutNeighbors(v))
                {
                    reversed.AddEdge(w, v);
                }
            }

            return reversed;
        }
    }
}
