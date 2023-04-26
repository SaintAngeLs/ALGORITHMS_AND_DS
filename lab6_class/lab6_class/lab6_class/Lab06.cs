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
            int n = g.VertexCount - 1;

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

            Dictionary<(int city, int keys), int> distances = new Dictionary<(int city, int keys), int>();
            Queue<(int city, int keys)> q = new Queue<(int city, int keys)>();
            HashSet<(int city, int keys)> visited = new HashSet<(int city, int keys)>();
            q.Enqueue((1, 0));
            visited.Add((1, 0));
            distances.Add((1, 0), 0);

            while (q.Count > 0)
            {
                var (u, currentKeys) = q.Dequeue();

                if (u == n)
                {
                    return (true, distances[(u, currentKeys)]);
                }

                foreach (var e in g.OutEdges(u))
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

                    int requiredKeys = 0;
                    (int city, int otherCity) = e.To < e.From ? (e.To, e.From) : (e.From, e.To);
                    if (borderGatesDict.TryGetValue((city, otherCity), out HashSet<int> requiredKeyColors))
                    {
                        foreach (int color in requiredKeyColors)
                        {
                            requiredKeys |= 1 << (color - 1);
                        }
                    }

                    int cost = e.Weight;
                    if ((currentKeys & requiredKeys) == requiredKeys)
                    {
                        var next = (v, newKeys);
                        if (!visited.Contains(next))
                        {
                            visited.Add(next);
                            distances.Add(next, distances[(u, currentKeys)] + cost);
                            q.Enqueue(next);
                        }
                    }
                }
            }

            // Return (false, 0) instead of (false, -1) to match the prompt
            return (false, 0);


        }


            }
}
  
