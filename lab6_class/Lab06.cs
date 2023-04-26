using ASD;
using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab06
{
    public class HeroesSolver : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - stwierdzenie, czy rozwiązanie istnieje
        /// </summary>
        /// <param name="g">graf przedstawiający mapę</param>
        /// <param name="keymasterTents">tablica krotek zawierająca pozycje namiotów klucznika - pierwsza liczba to kolor klucznika, druga to numer skrzyżowania</param>
        /// <param name="borderGates">tablica krotek zawierająca pozycje bram granicznych - pierwsza liczba to kolor bramy, dwie pozostałe to numery skrzyżowań na drodze między którymi znajduje się brama</param>
        /// <param name="p">ilość występujących kolorów (występujące kolory to 1,2,...,p)</param>
        /// <returns>bool - wartość true jeśli rozwiązanie istnieje i false wpp.</returns>
        public bool Lab06Stage1(Graph<int> g, (int color, int city)[] keymasterTents, (int color, int cityA, int cityB)[] borderGates, int p)
        {
            int n = g.VertexCount - 1;

            //// tests 1, 3, 7 are  
            //// Tworzymy słownik namiotów klucznika, klucz to kolor, wartość to zbiór skrzyżowań z namiotem klucznika w tym kolorze
            //Dictionary<int, HashSet<int>> keymasters = new Dictionary<int, HashSet<int>>();

            //foreach (var kt in keymasterTents)
            //{
            //    if (!keymasters.ContainsKey(kt.color))
            //    {
            //        keymasters.Add(kt.color, new HashSet<int>());
            //    }
            //    keymasters[kt.color].Add(kt.city);
            //}

            //// Tworzymy słownik bram granicznych
            //Dictionary<(int cityA, int cityB), HashSet<int>> borderGatesDict = new Dictionary<(int cityA, int cityB), HashSet<int>>();
            //foreach (var bg in borderGates)
            //{
            //    (int city1, int city2) = bg.cityA < bg.cityB ? (bg.cityA, bg.cityB) : (bg.cityB, bg.cityA);
            //    var gate = (city1, city2);
            //    if (!borderGatesDict.ContainsKey(gate))
            //    {
            //        borderGatesDict.Add(gate, new HashSet<int>());
            //    }
            //    borderGatesDict[gate].Add(bg.color);
            //}
            //// Sprawdzamy, czy istnieje przynajmniej jedna ścieżka między skrzyżowaniem 1 a n, na której można przejść przez wszystkie bramy
            //Queue<int> q = new Queue<int>();
            //HashSet<int> visited = new HashSet<int>();
            //Dictionary<(int, int), bool> canPass = new Dictionary<(int, int), bool>();
            //foreach (var e in g.DFS().SearchAll())
            //{
            //    int u = e.From;
            //    int v = e.To;
            //    canPass[(u, v)] = true;
            //    canPass[(v, u)] = true;
            //}

            //q.Enqueue(1);
            //visited.Add(1);
            //while (q.Count > 0)
            //{
            //    int u = q.Dequeue();
            //    foreach (var e in g.OutEdges(u))
            //    {
            //        int v = e.To;
            //        if (!visited.Contains(v))
            //        {
            //            visited.Add(v);
            //            q.Enqueue(v);
            //        }

            //        var edge = (u, v);
            //        if (!canPass.ContainsKey(edge))
            //        {
            //            canPass[edge] = true;
            //            canPass[(v, u)] = true;
            //        }
            //        if (borderGatesDict.ContainsKey((u, v)))
            //        {
            //            var colors = borderGatesDict[(u, v)];
            //            bool canPassBramka = true;
            //            foreach (var c in colors)
            //            {
            //                if (!keymasters.ContainsKey(c) || !keymasters[c].Contains(u))
            //                {
            //                    canPassBramka = false;
            //                }
            //            }
            //            if (!canPassBramka)
            //            {
            //                canPass[edge] = false;
            //                canPass[(v, u)] = false;
            //                continue;
            //            }
            //        }
            //    }
            //}

            //// Sprawdzamy, czy istnieje ścieżka między skrzyżowaniem 1 a n, na której można przejść przez wszystkie bramy
            //q.Clear();
            //visited.Clear();
            //q.Enqueue(1);
            //visited.Add(1);
            //while (q.Count > 0)
            //{
            //    int u = q.Dequeue();
            //    foreach (var e in g.OutEdges(u))
            //    {
            //        int v = e.To;
            //        if (visited.Contains(v)) continue;

            //        var edge = (u, v);
            //        if (canPass[edge])
            //        {
            //            visited.Add(v);
            //            q.Enqueue(v);
            //        }
            //    }
            //}

            //return visited.Contains(n);
            //===================================================================================================
            //var keymasterTentsDict = keymasterTents.ToDictionary(tent => tent.city, tent => tent.color);
            //var borderGatesDict = borderGates.ToDictionary(gate => (gate.cityA, gate.cityB), gate => gate.color);

            //var visited = new HashSet<(int, int)>();
            //var queue = new SortedSet<(int, int, int)>(Comparer<(int, int, int)>.Create((a, b) => a.Item1.CompareTo(b.Item1) == 0 ? a.Item2.CompareTo(b.Item2) : a.Item1.CompareTo(b.Item1)));
            //queue.Add((0, 1, 0));

            //while (queue.Count > 0)
            //{
            //    var current = queue.First();
            //    queue.Remove(current);

            //    var currentDistance = current.Item1;
            //    var currentCity = current.Item2;
            //    var currentKeys = current.Item3;

            //    if (visited.Contains((currentCity, currentKeys))) continue;
            //    visited.Add((currentCity, currentKeys));

            //    if (keymasterTentsDict.ContainsKey(currentCity))
            //    {
            //        int keyColor = keymasterTentsDict[currentCity];
            //        currentKeys |= 1 << (keyColor - 1);
            //    }

            //    if (currentCity == g.VertexCount) return true;

            //    foreach (var neighbor in g.OutNeighbors(currentCity))
            //    {
            //        int neighborCity = neighbor.Item1;
            //        int edgeDistance = neighbor.Item2;

            //        if (!borderGatesDict.TryGetValue((currentCity, neighborCity), out int gateColor) && !borderGatesDict.TryGetValue((neighborCity, currentCity), out gateColor))
            //        {
            //            queue.Add((currentDistance + edgeDistance, neighborCity, currentKeys));
            //        }
            //        else
            //        {
            //            int requiredKeyMask = 1 << (gateColor - 1);
            //            if ((currentKeys & requiredKeyMask) == requiredKeyMask)
            //            {
            //                queue.Add((currentDistance + edgeDistance, neighborCity, currentKeys));
            //            }
            //        }
            //    }
            //}

            //return false;

            //===================================================================================================
            //var keymasterTentsDict = new Dictionary<int, List<int>>();
            //foreach (var tent in keymasterTents)
            //{
            //    if (!keymasterTentsDict.ContainsKey(tent.city))
            //    {
            //        keymasterTentsDict[tent.city] = new List<int>();
            //    }
            //    keymasterTentsDict[tent.city].Add(tent.color);
            //}

            //var borderGatesDict = new Dictionary<(int, int), List<int>>();
            //foreach (var gate in borderGates)
            //{
            //    var key = (gate.cityA, gate.cityB);
            //    if (!borderGatesDict.ContainsKey(key))
            //    {
            //        borderGatesDict[key] = new List<int>();
            //    }
            //    borderGatesDict[key].Add(gate.color);
            //}

            //var visited = new HashSet<(int, int)>();
            //var queue = new SortedSet<(int, int, int)>(Comparer<(int, int, int)>.Create((a, b) => a.Item1.CompareTo(b.Item1) == 0 ? a.Item2.CompareTo(b.Item2) : a.Item1.CompareTo(b.Item1)));

            //queue.Add((0, 1, 0));

            //while (queue.Count > 0)
            //{
            //    var current = queue.First();
            //    queue.Remove(current);

            //    var currentDistance = current.Item1;
            //    var currentCity = current.Item2;
            //    var currentKeys = current.Item3;

            //    if (visited.Contains((currentCity, currentKeys))) continue;
            //    visited.Add((currentCity, currentKeys));

            //    if (keymasterTentsDict.TryGetValue(currentCity, out List<int> keyColors))
            //    {
            //        foreach (int keyColor in keyColors)
            //        {
            //            currentKeys |= 1 << (keyColor - 1);
            //        }
            //    }

            //    if (currentCity == g.VertexCount) return true;

            //    foreach (Edge<int> edge in g.OutEdges(currentCity))
            //    {
            //        int neighborCity = edge.To;
            //        if (borderGatesDict.TryGetValue((currentCity, neighborCity), out List<int> gateColors) || borderGatesDict.TryGetValue((neighborCity, currentCity), out gateColors))
            //        {
            //            bool gateBlocked = false;
            //            foreach (int gateColor in gateColors)
            //            {
            //                if ((currentKeys & (1 << (gateColor - 1))) == 0)
            //                {
            //                    gateBlocked = true;
            //                    break;
            //                }
            //            }
            //            if (gateBlocked) continue;
            //        }
            //    }
            //}

            //return false;
            //----------------------------------- nowy kod -----
            //var keymasterTentsDict = new Dictionary<int, int>();
            //foreach (var tent in keymasterTents)
            //{
            //    if (!keymasterTentsDict.ContainsKey(tent.city))
            //    {
            //        keymasterTentsDict[tent.city] = tent.color;
            //    }
            //}

            //var borderGatesDict = new Dictionary<(int, int), List<int>>();
            //foreach (var gate in borderGates)
            //{
            //    var key = (gate.cityA, gate.cityB);
            //    if (!borderGatesDict.ContainsKey(key))
            //    {
            //        borderGatesDict[key] = new List<int>();
            //    }
            //    borderGatesDict[key].Add(gate.color);
            //}

            //var queue = new Queue<(int city, int keys, int steps)>();
            //var visited = new HashSet<(int city, int keys)>();

            //queue.Enqueue((0, 0, 0));
            //visited.Add((0, 0));

            //while (queue.Count > 0)
            //{
            //    var (currentCity, currentKeys, currentSteps) = queue.Dequeue();

            //    if (currentCity == g.VertexCount - 1)
            //    {
            //        return true;
            //    }

            //    foreach (var neighbor in g.OutNeighbors(currentCity))
            //    {
            //        if (keymasterTentsDict.TryGetValue(neighbor, out int keymasterColor))
            //        {
            //            currentKeys |= 1 << (keymasterColor - 1);
            //        }

            //        if (borderGatesDict.TryGetValue((currentCity, neighbor), out List<int> gateColors) || borderGatesDict.TryGetValue((neighbor, currentCity), out gateColors))
            //        {
            //            bool gateBlocked = false;
            //            foreach (int gateColor in gateColors)
            //            {
            //                if ((currentKeys & (1 << (gateColor - 1))) == 0)
            //                {
            //                    gateBlocked = true;
            //                    break;
            //                }
            //            }
            //            if (gateBlocked) continue;
            //        }

            //        var newVisitedState = (neighbor, currentKeys);
            //        if (!visited.Contains(newVisitedState))
            //        {
            //            queue.Enqueue((neighbor, currentKeys, currentSteps + 1));
            //            visited.Add(newVisitedState);
            //        }
            //    }
            //}

            //return false;
            //=========================================
            /// =====GIT GIT ====== TEST 1 PRZECHODZI+++++++++   /// test 3, 6, 7 nie ;(

            //Dictionary<int, HashSet<int>> keymasters = new Dictionary<int, HashSet<int>>();

            //foreach (var kt in keymasterTents)
            //{
            //    if (!keymasters.ContainsKey(kt.color))
            //    {
            //        keymasters.Add(kt.color, new HashSet<int>());
            //    }
            //    keymasters[kt.color].Add(kt.city);
            //}

            //Dictionary<(int cityA, int cityB), HashSet<int>> borderGatesDict = new Dictionary<(int cityA, int cityB), HashSet<int>>();
            //foreach (var bg in borderGates)
            //{
            //    (int city1, int city2) = bg.cityA < bg.cityB ? (bg.cityA, bg.cityB) : (bg.cityB, bg.cityA);
            //    var gate = (city1, city2);
            //    if (!borderGatesDict.ContainsKey(gate))
            //    {
            //        borderGatesDict.Add(gate, new HashSet<int>());
            //    }
            //    borderGatesDict[gate].Add(bg.color);
            //}

            //Queue<(int city, int keys)> q = new Queue<(int city, int keys)>();
            //HashSet<(int city, int keys)> visited = new HashSet<(int city, int keys)>();

            //q.Enqueue((1, 0));
            //visited.Add((1, 0));

            //while (q.Count > 0)
            //{
            //    var (u, currentKeys) = q.Dequeue();

            //    if (u == n)
            //    {
            //        return true;
            //    }

            //    foreach (var e in g.OutEdges(u))
            //    {
            //        int v = e.To;
            //        if (keymasters.TryGetValue(v, out HashSet<int> keymasterCities))
            //        {
            //            foreach (int keymasterColor in keymasterCities)
            //            {
            //                currentKeys |= 1 << (keymasterColor - 1);
            //            }
            //        }

            //        var edge = (u, v);
            //        if (borderGatesDict.TryGetValue(edge, out HashSet<int> colors))
            //        {
            //            bool canPassGate = true;
            //            foreach (var c in colors)
            //            {
            //                if ((currentKeys & (1 << (c - 1))) == 0)
            //                {
            //                    canPassGate = false;
            //                    break;
            //                }
            //            }
            //            if (!canPassGate) continue;
            //        }

            //        var newState = (v, currentKeys);
            //        if (!visited.Contains(newState))
            //        {
            //            visited.Add(newState);
            //            q.Enqueue(newState);
            //        }
            //    }
            //}

            //return false;
            //======================================================================================

            //Dictionary<int, HashSet<int>> keymasters = new Dictionary<int, HashSet<int>>();

            //foreach (var kt in keymasterTents)
            //{
            //    if (!keymasters.ContainsKey(kt.color))
            //    {
            //        keymasters.Add(kt.color, new HashSet<int>());
            //    }
            //    keymasters[kt.color].Add(kt.city);
            //}

            //Dictionary<(int cityA, int cityB), HashSet<int>> borderGatesDict = new Dictionary<(int cityA, int cityB), HashSet<int>>();
            //foreach (var bg in borderGates)
            //{
            //    (int city1, int city2) = bg.cityA < bg.cityB ? (bg.cityA, bg.cityB) : (bg.cityB, bg.cityA);
            //    var gate = (city1, city2);
            //    if (!borderGatesDict.ContainsKey(gate))
            //    {
            //        borderGatesDict.Add(gate, new HashSet<int>());
            //    }
            //    borderGatesDict[gate].Add(bg.color);
            //}

            //Queue<(int city, int keys)> q = new Queue<(int city, int keys)>();
            //HashSet<(int city, int keys)> visited = new HashSet<(int city, int keys)>();

            //q.Enqueue((1, 0));
            //visited.Add((1, 0));

            //while (q.Count > 0)
            //{
            //    var (u, currentKeys) = q.Dequeue();

            //    if (u == n)
            //    {
            //        return true;
            //    }

            //    foreach (var e in g.OutEdges(u))
            //    {
            //        int v = e.To;
            //        if (keymasters.TryGetValue(v, out HashSet<int> keymasterCities))
            //        {
            //            foreach (int keymasterColor in keymasterCities)
            //            {
            //                currentKeys |= 1 << (keymasterColor - 1);
            //            }
            //        }

            //        var edge = (u, v);
            //        var reverseEdge = (v, u);
            //        if (borderGatesDict.TryGetValue(edge, out HashSet<int> colors) || borderGatesDict.TryGetValue(reverseEdge, out colors))
            //        {
            //            bool canPassGate = true;
            //            foreach (var c in colors)
            //            {
            //                if ((currentKeys & (1 << (c - 1))) == 0)
            //                {
            //                    canPassGate = false;
            //                    break;
            //                }
            //            }
            //            if (!canPassGate) continue;
            //        }

            //        var newState = (v, currentKeys);
            //        if (!visited.Contains(newState))
            //        {
            //            visited.Add(newState);
            //            q.Enqueue(newState);
            //        }
            //    }
            //}

            //return false;
            //===========================================================================================
            Dictionary<int, HashSet<int>> keymasters = new Dictionary<int, HashSet<int>>();

            foreach (var kt in keymasterTents)
            {
                if (!keymasters.ContainsKey(kt.color))
                {
                    keymasters.Add(kt.color, new HashSet<int>());
                }
                keymasters[kt.color].Add(kt.city);
            }

            Dictionary<(int cityA, int cityB), HashSet<int>> borderGatesDict = new Dictionary<(int cityA, int cityB), HashSet<int>>();
            foreach (var bg in borderGates)
            {
                (int city1, int city2) = bg.cityA < bg.cityB ? (bg.cityA, bg.cityB) : (bg.cityB, bg.cityA);
                var gate = (city1, city2);
                if (!borderGatesDict.ContainsKey(gate))
                {
                    borderGatesDict.Add(gate, new HashSet<int>());
                }
                borderGatesDict[gate].Add(bg.color);
            }

            Queue<(int city, int keys)> q = new Queue<(int city, int keys)>();
            HashSet<(int city, int keys)> visited = new HashSet<(int city, int keys)>();

            q.Enqueue((1, 0));
            visited.Add((1, 0));

            while (q.Count > 0)
            {
                var (u, currentKeys) = q.Dequeue();

                if (u == n)
                {
                    return true;
                }

                foreach (var e in g.DFS().SearchFrom(u))
                {
                    int v = e.To;

                    int newKeys = currentKeys;
                    if (keymasters.TryGetValue(v, out HashSet<int> keymasterCities))
                    {
                        foreach (int keymasterColor in keymasterCities)
                        {
                            newKeys |= 1 << (keymasterColor - 1);
                        }
                    }

                    var edge = (u, v);
                    var reverseEdge = (v, u);
                    if (borderGatesDict.TryGetValue(edge, out HashSet<int> colors) || borderGatesDict.TryGetValue(reverseEdge, out colors))
                    {
                        bool canPassGate = true;
                        foreach (var c in colors)
                        {
                            if ((newKeys & (1 << (c - 1))) == 0)
                            {
                                canPassGate = false;
                                break;
                            }
                        }
                        if (!canPassGate) continue;
                    }

                    var newState = (v, newKeys);
                    if (!visited.Contains(newState))
                    {
                        visited.Add(newState);
                        q.Enqueue(newState);
                    }
                }
            }

            return false;


        }


        /// <summary>
        /// Etap 2 - stwierdzenie, czy rozwiązanie istnieje
        /// </summary>
        /// <param name="g">graf przedstawiający mapę</param>
        /// <param name="keymasterTents">tablica krotek zawierająca pozycje namiotów klucznika - pierwsza liczba to kolor klucznika, druga to numer skrzyżowania</param>
        /// <param name="borderGates">tablica krotek zawierająca pozycje bram granicznych - pierwsza liczba to kolor bramy, dwie pozostałe to numery skrzyżowań na drodze między którymi znajduje się brama</param>
        /// <param name="p">ilość występujących kolorów (występujące kolory to 1,2,...,p)</param>
        /// <returns>krotka (bool solutionExists, int solutionLength) - solutionExists ma wartość true jeśli rozwiązanie istnieje i false wpp. SolutionLenth zawiera długość optymalnej trasy ze skrzyżowania 1 do n</returns>
        public (bool solutionExists, int solutionLength) Lab06Stage2(Graph<int> g, (int color, int city)[] keymasterTents, (int color, int cityA, int cityB)[] borderGates, int p)
        {

            int n = g.VertexCount - 1; // wierzchołek 0 nie występuje w zadaniu

            // Tworzenie tablicy reprezentującej graf z bramami granicznymi
            int[,] graph = new int[n + 1, n + 1];
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    graph[i, j] = int.MaxValue;
                }
            }

            foreach (var edge in g.DFS().SearchAll())
            {
                int u = edge.From;
                int v = edge.To;
                int weight = edge.Weight;

                graph[u, v] = weight;
                graph[v, u] = weight;
            }

            foreach (var gate in borderGates)
            {
                int color = gate.color;
                int u = gate.cityA;
                int v = gate.cityB;

                for (int i = 1; i <= p; i++)
                {
                    if (i == color)
                    {
                        continue;
                    }

                    graph[u, v] = int.MaxValue;
                    graph[v, u] = int.MaxValue;
                }
            }

            // Tworzenie tablicy reprezentującej graf z namiotami kluczników
            int[,] keymasterGraph = new int[n + 1, n + 1];
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    keymasterGraph[i, j] = int.MaxValue;
                }
            }

            foreach (var edge in g.DFS().SearchAll())
            {
                int u = edge.From;
                int v = edge.To;
                int weight = edge.Weight;

                keymasterGraph[u, v] = weight;
                keymasterGraph[v, u] = weight;
            }

            foreach (var tent in keymasterTents)
            {
                int color = tent.color;
                int u = tent.city;

                for (int i = 1; i <= p; i++)
                {
                    if (i == color)
                    {
                        continue;
                    }

                    keymasterGraph[0, u] = int.MaxValue;
                    keymasterGraph[u, 0] = int.MaxValue;
                }
            }

            // Wykorzystanie algorytmu Dijkstry do znalezienia najkrótszej ścieżki
            int[] distances = new int[n + 1];
            bool[] visited = new bool[n + 1];
            for (int i = 0; i <= n; i++)
            {
                distances[i] = int.MaxValue;
            }

            distances[1] = 0;

            for (int i = 0; i < n; i++)
            {
                int u = GetMinDistance(distances, visited);
                visited[u] = true;

                for (int v = 0; v <= n; v++)
                {
                    if (!visited[v] && graph[u, v] != int.MaxValue && distances[u] != int.MaxValue && distances[u] + graph[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + graph[u, v];
                    }
                }
            } // Sprawdzenie, czy istnieje rozwiązanie
            bool solutionExists = true; 
            for (int i = 0; i < borderGates.Length; i++)
            {
                int color = borderGates[i].color;
                int cityA = borderGates[i].cityA;
                int cityB = borderGates[i].cityB;

                if ((distances[cityA] == int.MaxValue && distances[cityB] == int.MaxValue))
                {
                    solutionExists = false;
                    break;
                }
            }

            if (!solutionExists)
            {
                return (false, 0);
            }

            // Obliczenie długości optymalnej trasy
            int solutionLength = distances[n];

            return (true, solutionLength);
        }

        private static int GetMinDistance(int[] distances, bool[] visited)
        {
            int minDistance = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < distances.Length; v++)
            {
                if (!visited[v] && distances[v] <= minDistance)
                {
                    minDistance = distances[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }
    }
}
  
