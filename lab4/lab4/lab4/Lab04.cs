using System;
using ASD.Graphs;
using ASD;
using System.Collections.Generic;
using System.Linq;

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

                gg.AddEdge(e.From * days_number + ((e.Weight - 1 + days_number) % days_number), e.To * days_number + e.Weight % days_number);
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
                    while (v != start_v * days_number + ((days_number + day - 1) % days_number))
                    {
                        v = S.Pop();
                        list.Insert(0, v / days_number);
                    }

                    return (true, list.ToArray());
                }
            }
            return (false, null);

            //// Initialize data structures for the search
            //var visited = new HashSet<int>();
            //var parent = new Dictionary<int, (int, int, int)>();
            //var queue = new Queue<int>();

            //// Enqueue the start vertex with the initial state
            //queue.Enqueue(start_v);
            //parent[start_v] = (-1, -1, day); // (previous vertex, previous edge weight, current day)

            //// Perform BFS search
            //while (queue.Count > 0)
            //{
            //    int curr_v = queue.Dequeue();
            //    visited.Add(curr_v);

            //    // Check if we have reached the destination vertex
            //    if (curr_v == end_v)
            //    {
            //        // Reconstruct the route by following the parent pointers
            //        var path = new List<int>();
            //        int v = end_v;
            //        while (v != start_v)
            //        {
            //            path.Add(v);
            //            v = parent[v].Item1;
            //        }
            //        path.Add(start_v);
            //        path.Reverse();
            //        return (true, path.ToArray());
            //    }

            //    // Expand the current vertex by considering all outgoing edges
            //    foreach (var edge in g.OutEdges(curr_v))
            //    {
            //        int next_v = edge.To;
            //        int weight = (int)edge.Weight;

            //        // Compute the next day based on the current day and edge weight
            //        int next_day = parent[curr_v].Item3 + weight;
            //        if (next_day >= days_number)
            //        {
            //            continue; // Cannot travel beyond the specified number of days
            //        }

            //        // Check if the next vertex has already been visited with a shorter path
            //        if (visited.Contains(next_v))
            //        {
            //            int prev_day = parent[next_v].Item3;
            //            if (next_day >= prev_day)
            //            {
            //                continue; // We have already visited next_v with a shorter path
            //            }
            //        }

            //        // Add the next vertex to the queue with updated parent and day
            //        queue.Enqueue(next_v);
            //        parent[next_v] = (curr_v, weight, next_day);
            //    }
            //}

            //// We have not found a path from start_v to end_v
            //return (false, null);

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
